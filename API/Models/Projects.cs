using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml;

namespace API.Models
{
    public class Projects
    {
        // verejné dátové zložky
        public string id { get; set; }
        public string name { get; set; }
        public string abbreviation { get; set; }
        public string customer { get; set; }

        // cesta k XML súboru so zdrojovými údajmi
        private static string xmlFile { get; set; } = ConfigurationManager.AppSettings["xmlFileProjects"];
        // štruktúra XML dokumentu
        private static string nodeString { get; set; } = "/projects/project";
        // objekt pre prácu s XML dokumentom
        private static XmlDocument doc = new XmlDocument();

        // metóda na načítanie zoznamu všetkých projektov
        public static List<Projects> GetAll()
        {
            DataSet ds = new DataSet(); // objekt dátovej sady
            ds.ReadXml(xmlFile);        // prečítanie zdrojových údajov z XML súboru

            List<Projects> items = new List<Projects>(); // vytvorenie zoznamu projektov
            // prejdenie celej sady dát a ich zápis do zoznamu items
            items = (from rows in ds.Tables[0].AsEnumerable()
                     select new Projects
                     {
                         // zápis všetkých údajov pre konkrétny projekt
                         id = rows[3].ToString(),
                         name = rows[0].ToString(),
                         abbreviation = rows[1].ToString(),
                         customer = rows[2].ToString()
                     }).ToList(); // pridanie projektu do zoznamu
            // vrátenie kompletného zoznamu
            return items;
        }

        // metóda na načítanie jedného konkrétneho projektu - vyhľadanie podľa id
        public static Projects GetOne(string id)
        {
            // vytvorenie nového objektu pre projekt
            Projects p = new Projects();
            // načítanie súboru do objektu doc
            doc.Load(xmlFile);
            // nájdenie projektu podľa id
            XmlNode project = (XmlElement)doc.SelectSingleNode($"{nodeString}[@id='{id}']");
            // ak sa nájde projekt, údaje sa uložia do objektu p
            if (project != null)
            {
                p.id = ((XmlElement)project).GetAttribute("id");
                p.name = project.SelectSingleNode("name").InnerText;
                p.abbreviation = project.SelectSingleNode("abbreviation").InnerText;
                p.customer = project.SelectSingleNode("customer").InnerText;
            }
            // vrátenie objektu (ak nenájde, vráti sa prázdny objekt)
            return p;
        }

        // metóda na pridanie nového projektu do zdrojového XML
        public static string Add(Projects p)
        {
            try
            {
                // načítanie existujúceho zoznamu
                doc.Load(xmlFile);

                XmlNode project = doc.CreateElement("project"); // element pre projekt
                XmlNode name = doc.CreateElement("name"); // element pre name
                XmlNode abbreviation = doc.CreateElement("abbreviation"); // element pre abbreviation
                XmlNode customer = doc.CreateElement("customer"); // element pre customer

                // nastavenie dát pre jednotlivé hodnoty projektu
                name.InnerText = p.name;
                abbreviation.InnerText = p.abbreviation;
                customer.InnerText = p.customer;
                // nastavenie atribútu id pre projekt
                ((XmlElement)project).SetAttribute("id", p.id);

                // pridanie child delementov k projektu
                project.AppendChild(name);
                project.AppendChild(abbreviation);
                project.AppendChild(customer);

                // pridanie projektu do root elementu
                doc.DocumentElement.AppendChild(project);

                // uloženie XML súboru
                doc.Save(xmlFile);
                return $"Projekt {p.id} bol pridaný.";
            }
            catch(Exception e)
            {
                // ak počas procesu pridania nastane nejaká chyba, vráti jej text
                return e.Message;
            }
        }

        // metóda na editáciu existujúceho projektu
        public static string Edit(Projects p)
        {
            try
            {
                // načítanie zdrojového XML súboru s dátami
                doc.Load(xmlFile);
                // nájdenie projektu pre editáciu
                XmlNode project = (XmlElement)doc.SelectSingleNode($"{nodeString}/[@id='{p.id}']");
                // ak sa našiel projekt na editáciu
                if(project != null)
                {
                    // nastavenie hodnôt pre editovaný projekt
                    project.SelectSingleNode("name").InnerText = p.name;
                    project.SelectSingleNode("abbreviation").InnerText = p.abbreviation;
                    project.SelectSingleNode("customer").InnerText = p.customer;

                    // Pozn.: id projektu nie je možné meniť, slúži ako identifikátor

                    // uloženie zdrojového XML súboru
                    doc.Save(xmlFile);

                    // vrátenie správy o úspešnej editácii
                    return $"Projekt {p.id} bol zmenený.";
                }
                else
                {
                    // ak sa nenašiel projekt na editáciu
                    return $"Projekt {p.id} sa nenašiel.";
                }
            }
            catch(Exception e)
            {
                // ak počas procesu editácie nastane nejaká chyba, vráti jej text
                return e.Message;
            }
        }

        // metóda na zmazanie existujúceho projektu
        public static string Del(string id)
        {
            try
            {
                // načítanie zdrojového XML súboru
                doc.Load(xmlFile);
                // vyhľadanie projektu na zmazanie
                XmlNode project = (XmlElement)doc.SelectSingleNode($"{nodeString}[@id='{id}']");
                // ak sa našiel projekt na zmazanie
                if (project != null)
                {
                    // zmazanie projektu zo zdrojového XML súboru
                    project.ParentNode.RemoveChild(project);
                    // uloženie zdrojového XML súboru
                    doc.Save(xmlFile);
                    // vrátenie správy o úspešnom zmazaní projektu
                    return $"Projekt {id} bol zmazaný.";
                }
                else
                {
                    // ak sa nenašiel projekt na zmazanie
                    return $"Projekt {id} sa nenašiel.";
                }
            }
            catch(Exception e)
            {
                // ak počas procesu zmazanie nastane nejaká chyba, vráti jej text
                return e.Message;
            }
        }
    }
}