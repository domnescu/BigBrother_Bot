using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace BigBrother_V2
{
    class Database
    {
        /// <summary>
        /// подключение локальной базы данных
        /// </summary>
        protected SQLiteConnection botDataBase;
        /// <summary>
        /// Объект который будет принимать SQl комманды
        /// </summary>
        protected static SQLiteCommand command;
        public Database()
        {
            botDataBase = new SQLiteConnection(string.Format("Data Source=BigBrother_V2_database.db; Journal_mode=WAL; Cache=Shared; Pooling=True; Max Pool Size=500;"));
        }

        /// <summary>
        /// Лазейка для добавление данных в БД по средствам сообщений ВК
        /// полученное сообщение обрабатывается как SQL запрос, 
        /// 
        /// </summary>
        /// <param name="Text"></param>
        public bool AddToDB(string Text)
        {
            botDataBase.Open();
            try
            {
                command = new SQLiteCommand(Text, botDataBase);
                command.ExecuteNonQuery();
            }
            catch
            {
                botDataBase.Close();
                return false;
            }
            botDataBase.Close();
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public string RandomResponse(string group)
        {
            botDataBase.Open();
            List<string> list = new List<string>();

            command = new SQLiteCommand("SELECT * FROM RandomAnswers WHERE Type='" + group + "';", botDataBase);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
                list.Add(reader.GetString(1));
            reader.Close();
            botDataBase.Close();
            return list[new Random().Next(list.Count)];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public List<long> GetListLong(string table, int column = 0)
        {
            List<long> list = new List<long>();
            botDataBase.Open();
            command = new SQLiteCommand("SELECT * FROM " + table + ";", botDataBase);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                list.Add(reader.GetInt64(column));
            }
            reader.Close();
            botDataBase.Close();
            return list;
        }

        /// <summary>
        /// Добавление диалога в БД для рассылки сообщений с информацией о передвижениях опера/чеснока/проверки
        /// </summary>
        /// <param name="peerID">Идентификатор диалога/беседы</param>
        public bool AddChat(long peerID)
        {
            botDataBase.Open();
            try
            {
                command = new SQLiteCommand("INSERT INTO Chats (PeerID) VALUES (" + peerID + ");", botDataBase);
                command.ExecuteNonQuery();
            }
            catch
            {
                botDataBase.Close();
                return false;
            }
            botDataBase.Close();
            return true;
        }
        /// <summary>
        /// Удаление диалога/беседы из БД для рассылки сообщений о передвижениях опера/чеснока/проверки
        /// </summary>
        /// <param name="peerID">Идентификатор диалога/беседы</param>
        public bool DeleteChat(long peerID)
        {
            botDataBase.Open();
            try
            {
                command = new SQLiteCommand("DELETE FROM Chats WHERE PeerID=" + peerID + ";", botDataBase);
                command.ExecuteNonQuery();
            }
            catch
            {
                botDataBase.Close();
                return false;
            }
            botDataBase.Close();
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public bool CheckText(string text, string table)
        {
            botDataBase.Open();
            command = new SQLiteCommand("SELECT * FROM " + table + ";", botDataBase);

            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                if (text.Contains(reader.GetString(0).ToLower()))
                {
                    reader.Close();
                    botDataBase.Close();
                    return true;
                }
            }
            reader.Close();
            botDataBase.Close();
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="variable"></param>
        /// <param name="value"></param>
        public void SetWorkingVariable(string variable, string value)
        {
            botDataBase.Open();
            try
            {
                command = new SQLiteCommand("UPDATE WorkingVariables SET value='" + value + "' WHERE variable='" + variable + "';", botDataBase);
                command.ExecuteNonQuery();
            }
            catch
            {
                botDataBase.Close();
            }
            botDataBase.Close();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="variable"></param>
        /// <returns></returns>
        public string GetWorkingVariable(string variable)
        {
            string temp = null;
            botDataBase.Open();
            command = new SQLiteCommand("SELECT * FROM WorkingVariables WHERE Variable='" + variable + "';", botDataBase);

            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                temp = reader.GetString(1);
            }
            reader.Close();
            botDataBase.Close();
            return temp;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        public void CleanTable(string table)
        {
            botDataBase.Open();
            command = new SQLiteCommand("DELETE FROM " + table, botDataBase);
            command.ExecuteNonQuery();
            botDataBase.Close();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public List<string> GetListString(string table, int column = 0)
        {
            List<string> list = new List<string>();
            botDataBase.Open();
            command = new SQLiteCommand("SELECT * FROM " + table, botDataBase);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                list.Add(reader.GetString(column));
            }
            reader.Close();
            botDataBase.Close();
            return list;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <param name="column0"></param>
        /// <param name="column1"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetDictionaryString(string table, int column0 = 0, int column1 = 1)
        {
            Dictionary<string, string> pairs = new Dictionary<string, string>();
            botDataBase.Open();
            command = new SQLiteCommand("SELECT * FROM " + table, botDataBase);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                pairs.Add(reader.GetString(column0), reader.GetString(column1));
            }
            reader.Close();
            botDataBase.Close();
            return pairs;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public int GetNrOfElements(string table)
        {
            botDataBase.Open();
            command = new SQLiteCommand("SELECT count(*) FROM " + table, botDataBase);
            int countRows = Convert.ToInt32(command.ExecuteScalar());
            botDataBase.Close();
            return countRows;
        }
        /// <summary>
        /// Проверяем кто набрал большее количество голосов 
        /// </summary>
        /// <returns>Имя опера который набрал максимальное количество голосов </returns>
        public string WhoIsNewOper()
        {
            string name = "";
            botDataBase.Open();
            command = new SQLiteCommand("SELECT OperName, COUNT(*) as count FROM Votes GROUP BY OperName ORDER BY count ASC;", botDataBase);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                name = reader.GetString(0);
            }
            reader.Close();
            botDataBase.Close();
            return name;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="table"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public bool CheckInt64(long value, string table, int column = 0)
        {
            botDataBase.Open();
            command = new SQLiteCommand("SELECT * FROM " + table + ";", botDataBase);

            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                if (reader.GetInt64(column) == value)
                {
                    reader.Close();
                    botDataBase.Close();
                    return true;
                }
            }
            reader.Close();
            botDataBase.Close();
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="operName"></param>
        public void AddVote(long UserID, string operName)
        {
            botDataBase.Open();
            command = new SQLiteCommand("INSERT INTO votes (UserID,OperName) VALUES (" + UserID + ",'" + operName + "');", botDataBase);
            command.ExecuteNonQuery();
            botDataBase.Close();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="location"></param>
        public void InfoUpdate(string Type, string location)
        {
            botDataBase.Open();
            command = new SQLiteCommand("UPDATE WarningList SET Location='" + location + "' WHERE Type='" + Type + "';", botDataBase);
            command.ExecuteNonQuery();
            botDataBase.Close();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="table">Название таблицы</param>
        /// <param name="column">Название столбца</param>
        /// <param name="cell">Данные в одной из ячеек из таблицы</param>
        /// <param name="NrOfColumn">Номер номер столбца из которого стоит извлечь данные</param>
        /// <returns></returns>
        public string GetString(string table, string column, string cell, int NrOfColumn)
        {
            botDataBase.Open();
            string location = null;
            command = new SQLiteCommand("SELECT * FROM " + table + " WHERE " + column + "='" + cell + "';", botDataBase);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                location = reader.GetString(NrOfColumn);
            }
            reader.Close();
            botDataBase.Close();
            return location;
        }
        /// <summary>
        /// Данный метод формирует группирует голоса из таблицы Votes по оперу
        /// </summary>
        /// <returns></returns>
        public string GetVoteStatus()
        {
            botDataBase.Open();
            string answer = null;
            command = new SQLiteCommand("SELECT OperName, COUNT(*) as count FROM Votes GROUP BY OperName ORDER BY count DESC;", botDataBase);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                answer += reader.GetString(0) + " - " + reader.GetInt32(1) + "\n";
            }
            reader.Close();
            botDataBase.Close();
            return answer;
        }


        /// <summary>
        /// Проверка в локальной БД информации о приёме пищи, о котором спрашивает пользователь
        /// </summary>
        /// <param name="time">Время приёма пищи</param>
        /// <returns>Информация о приёме пищи, о котором спрашивает пользователь. Если такая информация отсутствует в БД
        /// возвращается одна из случайных фраз, в которых бот просит чтобы ему сказали чем кормят в столовой</returns>
        public string GetMenu(string time)
        {
            string answerCaffeteria = null;
            botDataBase.Open();
            DateTime dateTime = DateTime.Now;
            //переменная в которой храниться день недели в виде строки (string)
            string day = DateTime.Now.Hour > 19 ? dateTime.AddDays(1).DayOfWeek.ToString() : dateTime.DayOfWeek.ToString();
            command = new SQLiteCommand("SELECT *  FROM CaffeteriaMemory WHERE Day='" + day + "' AND Time='" + time + "';", botDataBase);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                answerCaffeteria = reader.GetString(0);
            }
            reader.Close();
            botDataBase.Close();
            return answerCaffeteria;
        }
        /// <summary>
        /// Добавление в локальную БД информации о том чем кормят/будут кормить в столовой
        /// </summary>
        /// <param name="Text">Информация о том чем кормят/будут кормить в столовой</param>
        /// <param name="time">Время приёма пищи</param>
        public void AddToMenu(string Text, string time)
        {
            botDataBase.Open();
            string day = DateTime.Now.DayOfWeek.ToString();

            command = new SQLiteCommand("SELECT count(*) FROM CaffeteriaMemory WHERE Day='" + day + "' AND Time='" + time + "';", botDataBase);
            int countRows = Convert.ToInt32(command.ExecuteScalar());
            if (countRows == 0)
            {
                command = new SQLiteCommand("INSERT INTO CaffeteriaMemory (Eat,Time,Day) VALUES ('" + Text + "', '" + time + "', '" + day + "');", botDataBase);
            }
            else
            {
                command = new SQLiteCommand("UPDATE CaffeteriaMemory SET Eat='" + Text + "' WHERE Day='" + day + "' AND Time='" + time + "';", botDataBase);
            }
            command.ExecuteNonQuery();
            botDataBase.Close();
        }
        /// <summary>
        /// Получение списка Типов угроз
        /// </summary>
        /// <returns></returns>
        public List<string> GetWarningTypes()
        {
            List<string> list = new List<string>();
            botDataBase.Open();
            command = new SQLiteCommand("SELECT * FROM WarningList GROUP BY Type", botDataBase);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                list.Add(reader.GetString(1));
            }
            reader.Close();
            botDataBase.Close();
            return list;
        }
        /// <summary>
        /// Получение списка угроз конкретного типа
        /// </summary>
        /// <param name="Type">Тип угроз</param>
        /// <returns>Список угроз</returns>
        public List<string> GetWarnings(string Type)
        {
            List<string> list = new List<string>();
            botDataBase.Open();
            command = new SQLiteCommand("SELECT * FROM WarningList WHERE Type='" + Type + "';", botDataBase);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                list.Add(reader.GetString(0));
            }
            reader.Close();
            botDataBase.Close();
            return list;
        }
    }
}
