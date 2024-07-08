using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace Util
{
    public static class JsonHelper
    {
        private static string settingsFilePath = "C:\\MonitoringAndManagementApplication\\appsettings.json";//kullancıdan alınabilir.

        public static void SaveServiceSettings(string serviceKey, ServiceSettings serviceSettings)
        {
            try
            {
                dynamic allSettings = LoadAllSettings();
                if (allSettings == null)
                {
                    allSettings = new JObject();
                }

                string categoryName = serviceKey.Contains("Service") ? "Services" : "WebApis";
                if (!allSettings.ContainsKey(categoryName))
                {
                    allSettings[categoryName] = new JObject();
                }

                allSettings[categoryName][serviceKey] = JObject.FromObject(serviceSettings);

                string json = JsonConvert.SerializeObject(allSettings, Formatting.Indented);
                File.WriteAllText(settingsFilePath, json);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error saving settings: {ex.Message}");//loga
            }
        }

        public static dynamic LoadServiceSettings(string serviceKey)
        {
            try
            {
                dynamic allSettings = LoadAllSettings();
                if (allSettings != null)
                {
                    string categoryName = serviceKey.Contains("Service") ? "Services" : "WebApis";
                    if (allSettings.ContainsKey(categoryName) && allSettings[categoryName][serviceKey] != null)
                    {
                        return allSettings[categoryName][serviceKey];
                    }
                }
                return null;//log basmalıyız.
            }
            catch (Exception ex)
            {
                throw new Exception($"Error loading settings: {ex.Message}");//loga
            }
        }
        //kodların ortaklaştırılması.
        public static Dictionary<string, Dictionary<string, ServiceSettings>> LoadAllServiceSettings()
        {
            try
            {
                Dictionary<string, Dictionary<string, ServiceSettings>> categorizedSettings = new Dictionary<string, Dictionary<string, ServiceSettings>>();

                if (File.Exists(settingsFilePath))
                {//kondisyonlar kontrol edilebilir.
                    string json = File.ReadAllText(settingsFilePath);
                    var allSettings = JsonConvert.DeserializeObject<JObject>(json);

                    foreach (var category in allSettings.Properties()) 
                    {
                        string categoryName = category.Name;
                        foreach (var serviceKey in category.Value.Children<JProperty>())
                        {
                            ServiceSettings settings = serviceKey.Value.ToObject<ServiceSettings>();

                            if (!categorizedSettings.ContainsKey(categoryName))
                            {
                                categorizedSettings[categoryName] = new Dictionary<string, ServiceSettings>();
                            }

                            categorizedSettings[categoryName].Add(serviceKey.Name, settings);
                        }
                    }
                }

                return categorizedSettings;//boş olma durumu :)
            }
            catch (Exception ex)
            {
                throw new Exception($"Error loading all service settings: {ex.Message}");
            }
        }

        public static string GetServicePath(string serviceName)
        {
            try
            {
                dynamic allSettings = LoadAllSettings();
                if (allSettings != null)
                {
                    if (allSettings.ContainsKey("Services") && allSettings["Services"][serviceName] != null)
                    {
                        var serviceSettings = allSettings["Services"][serviceName];
                        string path = serviceSettings["FolderPath"];
                        return path;
                    }
                }
                return null; // If service or path not found, return null
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving service path: {ex.Message}");
            }
        }

        private static dynamic LoadAllSettings()
        {
            try
            {
                if (File.Exists(settingsFilePath))
                {
                    string json = File.ReadAllText(settingsFilePath);
                    return JsonConvert.DeserializeObject<JObject>(json);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error loading settings: {ex.Message}");
            }
        }
    }
}
