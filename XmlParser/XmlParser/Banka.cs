﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace XmlParser
{
    public class SerializeConfig<T> where T : class
    {
        public static void Serialize(string path, T type)
        {
            var serializer = new XmlSerializer(type.GetType());
            using (var writer = new FileStream(path, FileMode.Create))
            {
                serializer.Serialize(writer, type);
            }
        }

        public static T DeSerialize(string path)
        {
            T type;
            var serializer = new XmlSerializer(typeof(T));
            using (var reader = XmlReader.Create(path))
            {
                type = serializer.Deserialize(reader) as T;
            }
            return type;
        }
    }

	[XmlRoot(ElementName="banka", Namespace="http://bs.tcmb.gov.tr/bankaSubeTumListe/")]
	public class Banka 
    {
		[XmlElement(ElementName="adr", Namespace="http://bs.tcmb.gov.tr/bankaSubeTumListe/")]
		public string Adr { get; set; }

		[XmlElement(ElementName="bAd", Namespace="http://bs.tcmb.gov.tr/bankaSubeTumListe/")]
		public string BAd { get; set; }

		[XmlElement(ElementName="bIlAd", Namespace="http://bs.tcmb.gov.tr/bankaSubeTumListe/")]
		public string BIlAd { get; set; }

		[XmlElement(ElementName="bKd", Namespace="http://bs.tcmb.gov.tr/bankaSubeTumListe/")]
		public string BKd { get; set; }

		[XmlAttribute(AttributeName="sonIslemTuru")]
		public string SonIslemTuru { get; set; }

		[XmlAttribute(AttributeName="sonIslemZamani")]
		public string SonIslemZamani { get; set; }
	}

	[XmlRoot(ElementName="bankaSubeleri", Namespace="http://bs.tcmb.gov.tr/bankaSubeTumListe/")]
	public class BankaSubeleri 
    {
		[XmlElement(ElementName="banka", Namespace="http://bs.tcmb.gov.tr/bankaSubeTumListe/")]
		public Banka Banka { get; set; }

		[XmlAttribute(AttributeName="bKd")]
		public string BKd { get; set; }

		[XmlAttribute(AttributeName="sAdt")]
		public string SAdt { get; set; }

		[XmlElement(ElementName="sube", Namespace="http://bs.tcmb.gov.tr/bankaSubeTumListe/")]
		public List<Sube> Sube { get; set; }
	}

	[XmlRoot(ElementName="bankaSubeTumListe", Namespace="http://bs.tcmb.gov.tr/bankaSubeTumListe/")]
	public class BankaSubeTumListe 
    {
		[XmlElement(ElementName="bankaSubeleri", Namespace="http://bs.tcmb.gov.tr/bankaSubeTumListe/")]
		public List<BankaSubeleri> BankaSubeleri { get; set; }

		[XmlAttribute(AttributeName="tarih")]
		public string Tarih { get; set; }

		[XmlAttribute(AttributeName="xmlns")]
		public string Xmlns { get; set; }
	}

	[XmlRoot(ElementName="sube", Namespace="http://bs.tcmb.gov.tr/bankaSubeTumListe/")]
	public class Sube 
    {
		[XmlElement(ElementName="bKd", Namespace="http://bs.tcmb.gov.tr/bankaSubeTumListe/")]
		public string BKd { get; set; }

		[XmlElement(ElementName="sAd", Namespace="http://bs.tcmb.gov.tr/bankaSubeTumListe/")]
		public string SAd { get; set; }

		[XmlElement(ElementName="sIlAd", Namespace="http://bs.tcmb.gov.tr/bankaSubeTumListe/")]
		public string SIlAd { get; set; }

		[XmlElement(ElementName="sIlcAd", Namespace="http://bs.tcmb.gov.tr/bankaSubeTumListe/")]
		public string SIlcAd { get; set; }

		[XmlElement(ElementName="sIlcKd", Namespace="http://bs.tcmb.gov.tr/bankaSubeTumListe/")]
		public string SIlcKd { get; set; }

		[XmlElement(ElementName="sIlKd", Namespace="http://bs.tcmb.gov.tr/bankaSubeTumListe/")]
		public string SIlKd { get; set; }

		[XmlElement(ElementName="sKd", Namespace="http://bs.tcmb.gov.tr/bankaSubeTumListe/")]
		public string SKd { get; set; }

		[XmlAttribute(AttributeName="sonIslemTuru")]
		public string SonIslemTuru { get; set; }

		[XmlAttribute(AttributeName="sonIslemZamani")]
		public string SonIslemZamani { get; set; }

		[XmlElement(ElementName="tlf", Namespace="http://bs.tcmb.gov.tr/bankaSubeTumListe/")]
		public string Tlf { get; set; }

        [XmlElement(ElementName = "adr", Namespace = "http://bs.tcmb.gov.tr/bankaSubeTumListe/")]
        public string Adr { get; set; }

        [XmlElement(ElementName = "fks", Namespace = "http://bs.tcmb.gov.tr/bankaSubeTumListe/")]
        public string Fks { get; set; }

        [XmlElement(ElementName = "epst", Namespace = "http://bs.tcmb.gov.tr/bankaSubeTumListe/")]
        public string Epst { get; set; }      
	}
}

