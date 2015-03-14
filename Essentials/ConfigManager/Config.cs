using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace Essentials.ConfigManager
{
    delegate bool Checker(string[] path);

    class Config
    {
        private string filename;
        public string FileName
        {
            get
            {
                return filename;
            }
        }
        private XmlDocument document;

        public Config(string filename)
        {
            this.filename = filename;
            this.document = new XmlDocument();
            if (File.Exists(filename))
            {
                this.Load();
            }
            else
            {
                this.document.AppendChild(this.document.CreateXmlDeclaration("1.0", "utf-8", null));
            }
        }

        public void Load()
        {
            document.Load(filename);
        }

        public void Save()
        {
            string dirpath = Path.GetDirectoryName(filename);
            if (!Directory.Exists(dirpath)) Directory.CreateDirectory(dirpath);
            document.Save(filename);
        }

        public bool checkPath(string[] path)
        {
            return path != null && path.Length >= 1 && path[0] != null;
        }

        public XmlNode getNode(string[] path)
        {
            if (checkPath(path))
            {
                XmlNode node = this.document;
                for (int i = 0; i < path.Length; i++)
                {
                    if (node == null) return null;
                    node = node.SelectSingleNode(path[i]);
                }
                return node;
            }
            return null;
        }

        public void setNode(string[] path)
        {
            if (checkPath(path))
            {
                XmlNode node = this.document;
                XmlNode temp = null;
                for (int i = 0; i < path.Length; i++)
                {
                    temp = node.SelectSingleNode(path[i]);
                    if (temp != null)
                    {
                        node = temp;
                    }
                    else
                    {
                        node.AppendChild(document.CreateElement(path[i]));
                        node = node.SelectSingleNode(path[i]);
                    }
                }
            }
        }

        public bool isNode(string[] path)
        {
            return this.getNode(path) != null;
        }

        public void set(string[] path, object value)
        {
            if (!this.isNode(path)) this.setNode(path);
            this.getNode(path).InnerText = value.ToString();
        }

        public void setDefault(string[] path, object value, Checker check)
        {
            if (!check(path))
            {
                this.set(path, value);
            }
        }

        public string getString(string[] path)
        {
            XmlNode node = this.getNode(path);
            return node != null ? node.InnerText : null;
        }

        public bool isString(string[] path)
        {
            return this.isNode(path) ? !string.IsNullOrEmpty(getString(path)) : false;
        }

        public bool getBoolean(string[] path)
        {
            bool result;
            return bool.TryParse(this.getString(path), out result) ? result : false;
        }

        public bool isBoolean(string[] path)
        {
            bool result;
            return this.isNode(path) ? bool.TryParse(getString(path), out result) : false;
        }

        public int getInt(string[] path)
        {
            int result;
            return int.TryParse(this.getString(path), out result) ? result : 0;
        }

        public bool isInt(string[] path)
        {
            int result;
            return this.isNode(path) ? int.TryParse(getString(path), out result) : false;
        }
    }
}
