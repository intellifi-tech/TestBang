using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.IO;
using SQLite;

namespace TestBang.DataBasee
{
    class DataBase
    {
        public DataBase() 
        {
            CreateDataBase();
        }
        public static string documentsFolder()
        {
            string path;
            path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            Directory.CreateDirectory(path);
            return path;
        }
        public static void CreateDataBase()
        {
            var conn = new SQLiteConnection(System.IO.Path.Combine(documentsFolder(), "TestBang.db"), false);
            conn.CreateTable<MEMBER_DATA>();
            conn.CreateTable<OLUSTURULAN_TESTLER>();
            conn.CreateTable<DERS_PROGRAMI>();
            conn.CreateTable<ODEME_GECMISI>();
            conn.CreateTable<BILDIRIMLER>();
            conn.CreateTable<ARKADAS_OYUNU>();
            conn.Close();
        }

        #region MEMBER_DATA
        public static bool MEMBER_DATA_EKLE(MEMBER_DATA GelenDoluTablo)
        {
            try
            {
                var conn = new SQLiteConnection(System.IO.Path.Combine(documentsFolder(), "TestBang.db"), false);
                conn.Insert(GelenDoluTablo);
                conn.Close();
                return true;
            }
            catch(Exception Ex)
            {
                var aa = Ex.Message;
                return false;
            }
        }
        public static List<MEMBER_DATA> MEMBER_DATA_GETIR()
        {
            Atla:
            try
            {
                var conn = new SQLiteConnection(System.IO.Path.Combine(documentsFolder(), "TestBang.db"), false);
                var gelenler = conn.Query<MEMBER_DATA>("Select * From MEMBER_DATA");
                conn.Close();
                return gelenler;
            }
            catch (Exception Ex)
            {
                goto Atla;
                var aa = Ex.Message;
                return null;
            }
           
        }
        public static bool MEMBER_DATA_TEMIZLE()
        {
            try
            {
                var conn = new SQLiteConnection(System.IO.Path.Combine(documentsFolder(), "TestBang.db"), false);
                conn.Query<MEMBER_DATA>("Delete From MEMBER_DATA");
                conn.Close();
                return true;
            }
            catch (Exception e)
            {
                string ee = e.ToString();
                return false;
            }

        }
        public static bool MEMBER_DATA_Guncelle(MEMBER_DATA Tablo)
        {
            try
            {
                var conn = new SQLiteConnection(System.IO.Path.Combine(documentsFolder(), "TestBang.db"), false);
                conn.Update(Tablo);
                conn.Close();
                return true;
            }
            catch (Exception e)
            {
                string ee = e.ToString();
                return false;
            }

        }
        #endregion

        #region OLUSTURULAN_TESTLER
        public static bool OLUSTURULAN_TESTLER_EKLE(OLUSTURULAN_TESTLER GelenDoluTablo)
        {
            try
            {
                var conn = new SQLiteConnection(System.IO.Path.Combine(documentsFolder(), "TestBang.db"), false);
                conn.Insert(GelenDoluTablo);
                conn.Close();
                return true;
            }
            catch (Exception Ex)
            {
                var aa = Ex.Message;
                return false;
            }
        }
        public static List<OLUSTURULAN_TESTLER> OLUSTURULAN_TESTLER_GETIR()
        {
            Atla:
            try
            {
                var conn = new SQLiteConnection(System.IO.Path.Combine(documentsFolder(), "TestBang.db"), false);
                var gelenler = conn.Query<OLUSTURULAN_TESTLER>("Select * From OLUSTURULAN_TESTLER");
                conn.Close();
                return gelenler;
            }
            catch (Exception Ex)
            {
                goto Atla;
                var aa = Ex.Message;
                return null;
            }
        }

        public static List<OLUSTURULAN_TESTLER> OLUSTURULAN_TESTLER_GETIR_TestID(string TestID)
        {
            Atla:
            try
            {
                var conn = new SQLiteConnection(System.IO.Path.Combine(documentsFolder(), "TestBang.db"), false);
                var gelenler = conn.Query<OLUSTURULAN_TESTLER>("Select * From OLUSTURULAN_TESTLER WHERE id=?",TestID);
                conn.Close();
                return gelenler;
            }
            catch (Exception Ex)
            {
                goto Atla;
                var aa = Ex.Message;
                return null;
            }

        }


        public static List<OLUSTURULAN_TESTLER> OLUSTURULAN_TESTLER_GETIR_LocalID(int LocalID)
        {
            Atla:
            try
            {
                var conn = new SQLiteConnection(System.IO.Path.Combine(documentsFolder(), "TestBang.db"), false);
                var gelenler = conn.Query<OLUSTURULAN_TESTLER>("Select * From OLUSTURULAN_TESTLER WHERE localid=?", LocalID);
                conn.Close();
                return gelenler;
            }
            catch (Exception Ex)
            {
                goto Atla;
                var aa = Ex.Message;
                return null;
            }

        }

        public static bool OLUSTURULAN_TESTLER_TEMIZLE()
        {
            try
            {
                var conn = new SQLiteConnection(System.IO.Path.Combine(documentsFolder(), "TestBang.db"), false);
                conn.Query<OLUSTURULAN_TESTLER>("Delete From OLUSTURULAN_TESTLER");
                conn.Close();
                return true;
            }
            catch (Exception e)
            {
                string ee = e.ToString();
                return false;
            }

        }
        public static bool OLUSTURULAN_TESTLER_Guncelle(OLUSTURULAN_TESTLER Tablo)
        {
            try
            {
                var conn = new SQLiteConnection(System.IO.Path.Combine(documentsFolder(), "TestBang.db"), false);
                conn.Update(Tablo);
                conn.Close();
                return true;
            }
            catch (Exception e)
            {
                string ee = e.ToString();
                return false;
            }

        }
        #endregion

        #region DERS_PROGRAMI
        public static bool DERS_PROGRAMI_EKLE(DERS_PROGRAMI GelenDoluTablo)
        {
            try
            {
                var conn = new SQLiteConnection(System.IO.Path.Combine(documentsFolder(), "TestBang.db"), false);
                conn.Insert(GelenDoluTablo);
                conn.Close();
                return true;
            }
            catch (Exception Ex)
            {
                var aa = Ex.Message;
                return false;
            }
        }
        public static List<DERS_PROGRAMI> DERS_PROGRAMI_GETIR()
        {
            Atla:
            try
            {
                var conn = new SQLiteConnection(System.IO.Path.Combine(documentsFolder(), "TestBang.db"), false);
                var gelenler = conn.Query<DERS_PROGRAMI>("Select * From DERS_PROGRAMI");
                conn.Close();
                return gelenler;
            }
            catch (Exception Ex)
            {
                goto Atla;
                var aa = Ex.Message;
                return null;
            }
        }

        public static List<DERS_PROGRAMI> DERS_PROGRAMI_GETIR_TestID(string TestID)
        {
            Atla:
            try
            {
                var conn = new SQLiteConnection(System.IO.Path.Combine(documentsFolder(), "TestBang.db"), false);
                var gelenler = conn.Query<DERS_PROGRAMI>("Select * From DERS_PROGRAMI WHERE id=?", TestID);
                conn.Close();
                return gelenler;
            }
            catch (Exception Ex)
            {
                goto Atla;
                var aa = Ex.Message;
                return null;
            }

        }


        public static List<DERS_PROGRAMI> DERS_PROGRAMI_LocalID(int LocalID)
        {
            Atla:
            try
            {
                var conn = new SQLiteConnection(System.IO.Path.Combine(documentsFolder(), "TestBang.db"), false);
                var gelenler = conn.Query<DERS_PROGRAMI>("Select * From DERS_PROGRAMI WHERE localid=?", LocalID);
                conn.Close();
                return gelenler;
            }
            catch (Exception Ex)
            {
                goto Atla;
                var aa = Ex.Message;
                return null;
            }

        }

        public static bool DERS_PROGRAMI_TEMIZLE()
        {
            try
            {
                var conn = new SQLiteConnection(System.IO.Path.Combine(documentsFolder(), "TestBang.db"), false);
                conn.Query<DERS_PROGRAMI>("Delete From DERS_PROGRAMI");
                conn.Close();
                return true;
            }
            catch (Exception e)
            {
                string ee = e.ToString();
                return false;
            }

        }
        public static bool DERS_PROGRAMI_Guncelle(DERS_PROGRAMI Tablo)
        {
            try
            {
                var conn = new SQLiteConnection(System.IO.Path.Combine(documentsFolder(), "TestBang.db"), false);
                conn.Update(Tablo);
                conn.Close();
                return true;
            }
            catch (Exception e)
            {
                string ee = e.ToString();
                return false;
            }

        }
        #endregion

        #region ODEME_GECMISI
        public static bool ODEME_GECMISI_EKLE(ODEME_GECMISI GelenDoluTablo)
        {
            try
            {
                var conn = new SQLiteConnection(System.IO.Path.Combine(documentsFolder(), "TestBang.db"), false);
                conn.Insert(GelenDoluTablo);
                conn.Close();
                return true;
            }
            catch (Exception Ex)
            {
                var aa = Ex.Message;
                return false;
            }
        }
        public static List<ODEME_GECMISI> ODEME_GECMISI_GETIR()
        {
            Atla:
            try
            {
                var conn = new SQLiteConnection(System.IO.Path.Combine(documentsFolder(), "TestBang.db"), false);
                var gelenler = conn.Query<ODEME_GECMISI>("Select * From ODEME_GECMISI");
                conn.Close();
                return gelenler;
            }
            catch (Exception Ex)
            {
                goto Atla;
                var aa = Ex.Message;
                return null;
            }

        }

        public static List<ODEME_GECMISI> ODEME_GECMISI_GETIR_UZAKID(string UzakdbId)
        {
            Atla:
            try
            {
                var conn = new SQLiteConnection(System.IO.Path.Combine(documentsFolder(), "TestBang.db"), false);
                var gelenler = conn.Query<ODEME_GECMISI>("Select * From ODEME_GECMISI WHERE UzakDB_ID=?", UzakdbId);
                conn.Close();
                return gelenler;
            }
            catch (Exception Ex)
            {
                goto Atla;
                var aa = Ex.Message;
                return null;
            }

        }
        public static bool ODEME_GECMISI_TEMIZLE()
        {
            try
            {
                var conn = new SQLiteConnection(System.IO.Path.Combine(documentsFolder(), "TestBang.db"), false);
                conn.Query<ODEME_GECMISI>("Delete From ODEME_GECMISI");
                conn.Close();
                return true;
            }
            catch (Exception e)
            {
                string ee = e.ToString();
                return false;
            }

        }
        public static bool ODEME_GECMISI_Guncelle(ODEME_GECMISI Tablo)
        {
            try
            {
                var conn = new SQLiteConnection(System.IO.Path.Combine(documentsFolder(), "TestBang.db"), false);
                conn.Update(Tablo);
                conn.Close();
                return true;
            }
            catch (Exception e)
            {
                string ee = e.ToString();
                return false;
            }

        }
        #endregion

        #region BILDIRIMLER
        public static bool BILDIRIMLER_EKLE(BILDIRIMLER GelenDoluTablo)
        {
            try
            {
                var conn = new SQLiteConnection(System.IO.Path.Combine(documentsFolder(), "TestBang.db"), false);
                conn.Insert(GelenDoluTablo);
                conn.Close();
                return true;
            }
            catch (Exception Ex)
            {
                var aa = Ex.Message;
                return false;
            }
        }
        public static List<BILDIRIMLER> BILDIRIMLER_GETIR()
        {
            Atla:
            try
            {
                var conn = new SQLiteConnection(System.IO.Path.Combine(documentsFolder(), "TestBang.db"), false);
                var gelenler = conn.Query<BILDIRIMLER>("Select * From BILDIRIMLER");
                conn.Close();
                return gelenler;
            }
            catch (Exception Ex)
            {
                goto Atla;
                var aa = Ex.Message;
                return null;
            }
        }

       
        public static bool BILDIRIMLER_TEMIZLE()
        {
            try
            {
                var conn = new SQLiteConnection(System.IO.Path.Combine(documentsFolder(), "TestBang.db"), false);
                conn.Query<ODEME_GECMISI>("Delete From BILDIRIMLER");
                conn.Close();
                return true;
            }
            catch (Exception e)
            {
                string ee = e.ToString();
                return false;
            }

        }
        public static bool BILDIRIMLER_Guncelle(BILDIRIMLER Tablo)
        {
            try
            {
                var conn = new SQLiteConnection(System.IO.Path.Combine(documentsFolder(), "TestBang.db"), false);
                conn.Update(Tablo);
                conn.Close();
                return true;
            }
            catch (Exception e)
            {
                string ee = e.ToString();
                return false;
            }

        }
        #endregion

        #region ARKADAS_OYUNU
        public static bool ARKADAS_OYUNU_EKLE(ARKADAS_OYUNU GelenDoluTablo)
        {
            try
            {
                var conn = new SQLiteConnection(System.IO.Path.Combine(documentsFolder(), "TestBang.db"), false);
                conn.Insert(GelenDoluTablo);
                conn.Close();
                return true;
            }
            catch (Exception Ex)
            {
                var aa = Ex.Message;
                return false;
            }
        }
        public static List<ARKADAS_OYUNU> ARKADAS_OYUNU_GETIR()
        {
            Atla:
            try
            {
                var conn = new SQLiteConnection(System.IO.Path.Combine(documentsFolder(), "TestBang.db"), false);
                var gelenler = conn.Query<ARKADAS_OYUNU>("Select * From ARKADAS_OYUNU");
                conn.Close();
                return gelenler;
            }
            catch (Exception Ex)
            {
                goto Atla;
                var aa = Ex.Message;
                return null;
            }
        }


        public static bool ARKADAS_OYUNU_TEMIZLE()
        {
            try
            {
                var conn = new SQLiteConnection(System.IO.Path.Combine(documentsFolder(), "TestBang.db"), false);
                conn.Query<ODEME_GECMISI>("Delete From ARKADAS_OYUNU");
                conn.Close();
                return true;
            }
            catch (Exception e)
            {
                string ee = e.ToString();
                return false;
            }

        }
        public static bool ARKADAS_OYUNU_Guncelle(BILDIRIMLER Tablo)
        {
            try
            {
                var conn = new SQLiteConnection(System.IO.Path.Combine(documentsFolder(), "TestBang.db"), false);
                conn.Update(Tablo);
                conn.Close();
                return true;
            }
            catch (Exception e)
            {
                string ee = e.ToString();
                return false;
            }

        }
        #endregion

    }
}