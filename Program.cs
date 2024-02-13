using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ManagerPassword
{
    public static class EncryptionKeyGenerator
    {
        public static string GenerateEncryptionKey(string keyFile, int keySize)
        {
            if (File.Exists(keyFile))
            {
                string key = File.ReadAllText(keyFile).Trim();
                return key;
            }
            else
            {
                string key = GenerateRandomKey(keySize);
                File.WriteAllText(keyFile, key);
                return key;
            }
        }

        private static string GenerateRandomKey(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            string key = new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            return key;
        }
    }

    public class Site
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }

    public class Program
    {
        static List<Site> sites = new List<Site>();
        static string keyFile = "encryption_key.txt";
        static string encryptionKey = EncryptionKeyGenerator.GenerateEncryptionKey(keyFile, 16);

        static void Main(string[] args)
        {
            // Загрузить данные из файла JSON
            LoadData(); 

            while (true)
            {
                ClearConsole();

                Console.WriteLine("===========[ Менеджер паролей ]===========");
                Console.WriteLine("1.   Добавить сайт и пароль");
                Console.WriteLine("2.   Удалить сайт и пароль");
                Console.WriteLine("3.   Изменить пароль");
                Console.WriteLine("4.   Изменить сайт");
                Console.WriteLine("5.   Получить пароль от сайта");
                Console.WriteLine("6.   Вывести список всех сайтов");
                Console.WriteLine("7.   Вывести список всех сайтов и паролей");
                Console.WriteLine("8.   Сохранить изменения");
                Console.WriteLine("255. Выйти");

                Console.Write("> ");
                byte c;
                // Блок ниже можно оставить так, он работает стабильно
                if (byte.TryParse(Console.ReadLine(), out c))
                {
                    // Ввод корректен, значение успешно преобразовано в byte
                    // Можете продолжить работу с переменной c
                }
                else
                {
                    // Ввод некорректен, значение не может быть преобразовано в byte
                    // Обработайте эту ситуацию по вашему усмотрению
                }

                string nameSite, password;
                switch (c)
                {
                    case 1:
                        Console.WriteLine("=== Добавление сайта ===");
                        Console.Write("Введите название сайта: ");
                        nameSite = Console.ReadLine();
                        Console.Write("Введите пароль: ");
                        password = Console.ReadLine();
                        sites.Add(new Site() { Name = nameSite, Password = password });
                        Console.WriteLine("[Сайт сохранен]");
                        Console.WriteLine("[Нажмите ENTER чтобы продолжить работу с меню программы]");
                        break;
                    case 2:
                        Console.WriteLine("=== Удаление сайта ===");
                        Console.Write("Введите название сайта: ");
                        nameSite = Console.ReadLine();
                        sites.Remove(sites.Find(x => x.Name == nameSite));
                        Console.WriteLine("[Сайт удален]");
                        Console.WriteLine("[Нажмите ENTER чтобы продолжить работу с меню программы]");
                        break;
                    case 3:
                        Console.WriteLine("=== Изменение пароля ===");
                        Console.Write("Введите название сайта: ");
                        nameSite = Console.ReadLine();
                        Console.Write("Введите новый пароль: ");
                        password = Console.ReadLine();
                        Site site = sites.Find(x => x.Name == nameSite);
                        if (site != null)
                        {
                            site.Password = password;
                            Console.WriteLine("[Пароль изменен]");
                        }
                        else
                        {
                            Console.WriteLine("[Пароль не изменен]");
                            Console.WriteLine("[Возможно вы ошиблись в имени сайта]");
                        }
                        Console.WriteLine("[Нажмите ENTER чтобы продолжить работу с меню программы]");
                        break;
                    case 4:
                        Console.WriteLine("=== Изменение сайта ===");
                        Console.Write("Введите название сайта: ");
                        nameSite = Console.ReadLine();
                        Console.Write("Введите новое название сайта: ");
                        string newNameSite = Console.ReadLine();
                        Site _site = sites.Find(x => x.Name == nameSite);
                        if (_site != null)
                        {
                            _site.Name = newNameSite;
                            Console.WriteLine("[Название сайта изменено]");
                        }
                        else
                        {
                            Console.WriteLine("[Название сайта не изменено]");
                            Console.WriteLine("[Возможно, вы ошиблись в имени сайта]");
                        }
                        Console.WriteLine("[Нажмите ENTER чтобы продолжить работу с меню программы]");
                        break;
                    case 5:
                        Console.WriteLine("=== Получить пароль от сайта ===");
                        Console.Write("Введите название сайта: ");
                        nameSite = Console.ReadLine();
                        Site foundSite = sites.Find(x => x.Name == nameSite);
                        if (foundSite != null)
                            Console.WriteLine("[Пароль от сайта '{0}': {1}]", foundSite.Name, foundSite.Password);
                        else
                        {
                            Console.WriteLine("[Сайт не найден]");
                            Console.WriteLine("[Возможно, вы ошиблись в имени сайта]");
                        }
                        Console.WriteLine("[Нажмите ENTER чтобы продолжить работу с меню программы]");
                        break;
                    case 6:
                        Console.WriteLine("=== Вывести список всех сайтов ===");
                        if (sites.Count > 0)
                            foreach (Site s in sites)
                                Console.WriteLine("Сайт: {0}", s.Name);
                        else
                            Console.WriteLine("[Список сайтов пуст]");
                        Console.WriteLine("[Нажмите ENTER чтобы продолжить работу с меню программы]");
                        break;
                    case 7:
                        Console.WriteLine("=== Вывести список всех сайтов и паролей ===");
                        if (sites.Count > 0)
                            foreach (Site s in sites)
                                Console.WriteLine("Сайт: {0}, Пароль: {1}", s.Name, s.Password);
                        else
                            Console.WriteLine("[Список сайтов пуст]");
                        Console.WriteLine("[Нажмите ENTER чтобы продолжить работу с меню программы]");
                        break;
                    case 8:
                        Console.WriteLine("[Изменения сохранены]");
                        SaveData();
                        Console.WriteLine("[Нажмите ENTER чтобы продолжить работу с меню программы]");
                        break;
                    case 255:
                        Console.WriteLine("До свидания!");
                        return;
                    default:
                        Console.WriteLine("[Неверный ввод]");
                        Console.WriteLine("[Нажмите ENTER чтобы продолжить работу с меню программы]");
                        break;
                }
                Console.ReadLine();
            }
        }

        static void ClearConsole()
        {
            Console.Clear();
        }

        static void SaveData()
        {
            if (sites.Count != 0)
            {
                string json = JsonConvert.SerializeObject(sites, Formatting.Indented);
                string encryptedData = Encrypt(json, encryptionKey);
                File.WriteAllText("data.json", encryptedData);
                Console.WriteLine("[Данные успешно сохранены]");
            }
        }

        static void LoadData()
        {
            if (File.Exists("data.json"))
            {
                string encryptedData = File.ReadAllText("data.json");
                string json = Decrypt(encryptedData, encryptionKey);
                sites = JsonConvert.DeserializeObject<List<Site>>(json);
                Console.WriteLine("[Данные успешно загружены]");
            }
            else
                Console.WriteLine("[Файл данных не найден]");
        }

        private static string Encrypt(string text, string key)
        {
            byte[] encryptedBytes;
            using (AesManaged aes = new AesManaged())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = Encoding.UTF8.GetBytes(key.Substring(0, 16));
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                byte[] textBytes = Encoding.UTF8.GetBytes(text);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        cs.Write(textBytes, 0, textBytes.Length);
                        cs.FlushFinalBlock();
                        encryptedBytes = ms.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(encryptedBytes);
        }

        private static string Decrypt(string cipher, string key)
        {
            byte[] cipherBytes = Convert.FromBase64String(cipher);
            byte[] decryptedBytes;
            using (AesManaged aes = new AesManaged())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = Encoding.UTF8.GetBytes(key.Substring(0, 16));
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using (MemoryStream ms = new MemoryStream(cipherBytes))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cs))
                        {
                            decryptedBytes = Encoding.UTF8.GetBytes(sr.ReadToEnd());
                        }
                    }
                }
            }
            return Encoding.UTF8.GetString(decryptedBytes);
        }

    }
}