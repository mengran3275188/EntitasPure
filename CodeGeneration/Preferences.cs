using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using DesperateDevs.Logging;

namespace Entitas.CodeGeneration
{
    public class Preferences
    {
        private static Preferences _sharedInstance;

        private static readonly Logger _logger = fabl.GetLogger(typeof(Preferences).FullName);

        private readonly string _propertiesPath;

        private readonly string _userPropertiesPath;

        private Properties _properties;

        private Properties _userProperties;

        public static Preferences sharedInstance
        {
            get
            {
                if (Preferences._sharedInstance == null)
                {
                    Preferences._sharedInstance = new Preferences((string)null, (string)null);
                }
                Preferences._sharedInstance.Reload();
                return Preferences._sharedInstance;
            }
            set
            {
                Preferences._sharedInstance = value;
            }
        }

        public string propertiesPath
        {
            get
            {
                return this._propertiesPath;
            }
        }

        public string userPropertiesPath
        {
            get
            {
                return this._userPropertiesPath;
            }
        }

        public bool propertiesExist
        {
            get
            {
                return File.Exists(this._propertiesPath);
            }
        }

        public bool userPropertiesExist
        {
            get
            {
                return File.Exists(this._userPropertiesPath);
            }
        }

        public string[] keys
        {
            get
            {
                return this.getMergedProperties().keys;
            }
        }

        public Properties properties
        {
            get
            {
                return this._properties;
            }
        }

        public Properties userProperties
        {
            get
            {
                return this._userProperties;
            }
        }

        public string this[string key]
        {
            get
            {
                return this.getMergedProperties()[key];
            }
            set
            {
                if (this._properties.HasKey(key) && !(value != this[key]))
                {
                    return;
                }
                this._properties[key] = value;
            }
        }

        public Preferences(string propertiesPath, string userPropertiesPath)
        {
            this._propertiesPath = (propertiesPath ?? Preferences.findFilePath("*.properties") ?? "Preferences.properties");
            this._userPropertiesPath = (userPropertiesPath ?? Preferences.findFilePath("*.userproperties") ?? (Environment.UserName + ".userproperties"));
            this.Reload();
        }

        protected Preferences(Properties properties, Properties userProperties)
        {
            this._properties = properties;
            this._userProperties = userProperties;
        }

        public void Reload()
        {
            this._properties = Preferences.loadProperties(this._propertiesPath);
            this._userProperties = Preferences.loadProperties(this._userPropertiesPath);
        }

        public void Save(bool minified = false)
        {
            File.WriteAllText(this._propertiesPath, minified ? this._properties.ToMinifiedString() : this._properties.ToString());
            File.WriteAllText(this._userPropertiesPath, minified ? this._userProperties.ToMinifiedString() : this._userProperties.ToString());
        }

        public bool HasKey(string key)
        {
            if (!this._properties.HasKey(key))
            {
                return this._userProperties.HasKey(key);
            }
            return true;
        }

        public void Reset(bool resetUser = false)
        {
            this._properties = new Properties();
            if (resetUser)
            {
                this._userProperties = new Properties();
            }
        }

        public override string ToString()
        {
            return this.getMergedProperties().ToString();
        }

        public static string[] FindAll(string searchPattern)
        {
            return Directory.GetFiles(Directory.GetCurrentDirectory(), searchPattern, SearchOption.TopDirectoryOnly);
        }

        private static string findFilePath(string searchPattern)
        {
            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory(), searchPattern, SearchOption.TopDirectoryOnly);
            string text = files.FirstOrDefault();
            if (files.Length > 1)
            {
                Preferences._logger.Warn("Found multiple files matching " + searchPattern + ":\n" + string.Join("\n", files.Select(Path.GetFileName).ToArray()) + "\nUsing " + text);
            }
            return text;
        }

        private static Properties loadProperties(string path)
        {
            return new Properties(File.Exists(path) ? File.ReadAllText(path) : string.Empty);
        }

        private Properties getMergedProperties()
        {
            Properties properties = new Properties(this._properties.ToDictionary());
            properties.AddProperties(this._userProperties.ToDictionary(), true);
            return properties;
        }
    }
}
