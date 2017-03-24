using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace XmlParser
{
    public class DbOperations
    {
        static List<BankaSubeleri> bankBranchs = new List<BankaSubeleri>();
        static List<Sube> branchs = new List<Sube>();
        static List<Mapping> mappingData = new List<Mapping>();
        static string sqlStatement = String.Empty, temp = String.Empty;
        public static bool result = false;

        public static void CreateTable()
        {
            sqlStatement = @" 
                            -- Create Table as XML_PARSER_BANKS
                            If Not Exists (Select id From sysobjects (Nolock) Where name ='XML_PARSER_BANKS' And type = 'U')
                            BEGIN
                            CREATE TABLE [dbo].XML_PARSER_BANKS (  
                                            bKd varchar(300) NOT NULL PRIMARY KEY,
				                            bAd varchar(300) NOT NULL,
				                            bIlAd varchar(300) NOT NULL,
                                            adr varchar(300) NOT NULL,
				                            sonIslemTuru varchar(300) NOT NULL,
	                                        sonIslemZamani varchar(300) NOT NULL)
                            END  
                         
                            -- Create Table as XML_PARSER_BRANCHS                          
                            If Not Exists (Select id From sysobjects (Nolock) Where name ='XML_PARSER_BRANCHS' And type = 'U')
                            BEGIN
                            CREATE TABLE [dbo].XML_PARSER_BRANCHS (  
                                            sKd varchar(300) NOT NULL,
				                            bKd varchar(300) NOT NULL FOREIGN KEY REFERENCES XML_PARSER_BANKS(bKd),
				                            sAd varchar(300) NOT NULL,
				                            sIlAd varchar(300),
				                            sIlcAd varchar(300),
                                            sIlcKd varchar(300),
				                            sIlKd varchar(300) NOT NULL,
	                                        tlf varchar(300),
				                            adr varchar(300),
                                            fks varchar(300),
				                            epst varchar(300),
				                            sonIslemTuru varchar(300) NOT NULL,
	                                        sonIslemZamani varchar(300) NOT NULL)
                            END";

            using (SqlConnection connection = new SqlConnection("data source=yourDb;persist security info=True;initial catalog=yourcatalog;User id=yourid;Password=yourpass"))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
            sqlStatement = String.Empty;
        }

        public static void RefreshData(BankaSubeTumListe values)
        {
            GetMappingData();

            foreach (var value in values.BankaSubeleri)
            {
                foreach (var data in mappingData)
                {
                    if (value.Banka.BKd == data.BankCode && value.Banka.BAd != data.BankName)
                    {
                        value.Banka.BAd = data.BankName; 
                    }
                }
            }
        }

        public static void GetMappingData()
        {
            sqlStatement = @" 
                             Select X.INSTITUTION_ID, X.ATT,I.FULL_NAME from INSTITUTIONS_X X(NOLOCK), INSTITUTIONS I (NOLOCK) where ATT!=0 AND I.INSTITUTION_ID=X.INSTITUTION_ID              
                            ";

            using (SqlConnection connection = new SqlConnection("data source=yourDb;persist security info=True;initial catalog=yourcatalog;User id=yourid;Password=yourpass")
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);
                connection.Open();

                using (SqlDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Mapping data = new Mapping();
                        data.InstitutionId = dr["INSTITUTION_ID"].ToString();
                        data.BankName = dr["FULL_NAME"].ToString(); 
                        if (dr["ATT"].ToString().Length == 4)
                            data.BankCode = dr["ATT"].ToString();
                        else
                        {
                            for(int i = dr["ATT"].ToString().Length; i < 4; i++)
                            {
                                data.BankCode += "0";
                            }
                            data.BankCode += dr["ATT"].ToString();
                        }
                        mappingData.Add(data);
                    }
                    connection.Close();
                }
            }
            sqlStatement = String.Empty;
        }

        public static void AddDatabase(BankaSubeTumListe values)
        {
            RefreshData(values);

            foreach (var value in values.BankaSubeleri)
            {               
                temp = @"          
			            Insert into XML_PARSER_BANKS (bKd,bAd,bIlAd,adr,sonIslemTuru, sonIslemZamani)
                        Values('@bKd','@bAd','@bIlAd','@adr','@sonIslemTuru','@sonIslemZamani')
			    ";

                temp = temp.Replace("@bKd", value.Banka.BKd.Trim());
                temp = temp.Replace("@bAd", value.Banka.BAd.Trim());
                temp = temp.Replace("@bIlAd", value.Banka.BIlAd.Trim());
                temp = temp.Replace("@adr", value.Banka.Adr.Trim());
                temp = temp.Replace("@sonIslemTuru", value.Banka.SonIslemTuru.Trim());
                temp = temp.Replace("@sonIslemZamani", value.Banka.SonIslemZamani.Trim());
                sqlStatement += temp;

                branchs = value.Sube;
                foreach (var branch in branchs)
                {
                    temp = @"
                          Insert into XML_PARSER_BRANCHS (sKd,bKd,sAd,sIlAd,sIlcAd,sIlcKd,sIlKd,tlf,adr,fks,epst,sonIslemTuru,sonIslemZamani)
                          Values('@sKd','@bKd','@sAd','@sIlAd','@sIlcAd','@sIlcKd','@sIlKd','@tlf','@adr','@fks','@epst','@sonIslemTuru','@sonIslemZamani')
                    ";

                    temp = temp.Replace("@sKd", branch.SKd.Trim());
                    temp = temp.Replace("@bKd", branch.BKd.Trim());
                    temp = temp.Replace("@sAd", branch.SAd.Trim());
                    temp = temp.Replace("@sIlAd", branch.SIlAd);
                    temp = temp.Replace("@sIlcAd", branch.SIlcAd);
                    temp = temp.Replace("@sIlcKd", branch.SIlcKd);
                    temp = temp.Replace("@sIlKd", branch.SIlKd.Trim());
                    temp = temp.Replace("@tlf", branch.Tlf);
                    temp = temp.Replace("@adr", branch.Adr);
                    temp = temp.Replace("@fks", branch.Fks);
                    temp = temp.Replace("@epst", branch.Epst);
                    temp = temp.Replace("@sonIslemTuru", branch.SonIslemTuru.Trim());
                    temp = temp.Replace("@sonIslemZamani", branch.SonIslemZamani.Trim());
                    sqlStatement += temp;
                }

                try
                {
                    using (SqlConnection connection = new SqlConnection("data source=yourDb;persist security info=True;initial catalog=yourcatalog;User id=yourid;Password=yourpass")
                    {
                        SqlCommand command = new SqlCommand(sqlStatement, connection);
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                    sqlStatement = String.Empty;
                    result = true;
                }
                catch
                {
                    MessageBox.Show("Dosyada sorunlu veri var.");
                    result = false;
                    sqlStatement = String.Empty;
                    break;
                }
            }      
        }
    }
}
