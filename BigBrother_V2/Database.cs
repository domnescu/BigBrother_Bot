using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace BigBrother_V2
{
    internal class Database
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
        /// </summary>
        /// <param name="Text"></param>
        public bool AddToDB(string Text)
        {
            botDataBase.Open();
            try
            {
                command = new SQLiteCommand(Text, botDataBase);
                _ = command.ExecuteNonQuery();
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
        /// Возвращение рандомного ответа из указанной группы ответов
        /// </summary>
        /// <param name="group">Группа ответов</param>
        /// <returns>Рандомная строка</returns>
        public string RandomResponse(string group)
        {
            botDataBase.Open();
            List<string> list = new();

            command = new SQLiteCommand("SELECT * FROM RandomAnswers WHERE Type='" + group + "';", botDataBase);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                list.Add(reader.GetString(1));
            }

            reader.Close();
            botDataBase.Close();
            return list[new Random().Next(list.Count)];
        }
        /// <summary>
        /// Получение списка строк из таблица
        /// </summary>
        /// <param name="table">Название таблицы</param>
        /// <param name="column">Номер столбца из которого нужно получить данные</param>
        /// <returns></returns>
        public List<long> GetListLong(string table, int column = 0, string condition = "")
        {
            List<long> list = new();
            botDataBase.Open();
            command = new SQLiteCommand("SELECT * FROM " + table + " " + condition, botDataBase);
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
        public bool AddChat(long peerID, string platform)
        {
            botDataBase.Open();
            try
            {
                command = new SQLiteCommand("INSERT INTO Chats (PeerID,Platform) VALUES (" + peerID + ",'" + platform + "');", botDataBase);
                _ = command.ExecuteNonQuery();
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
                _ = command.ExecuteNonQuery();
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
        /// Проверка текста на содержание элементов из таблицы
        /// </summary>
        /// <param name="text">Текст который следует проверить</param>
        /// <param name="table">Таблица с значениями которой нужно сравнивать</param>
        /// <returns>True - если в тексте содержится элемент из таблицы
        /// False - Если в тексте не содержится не один элемент из таблицы</returns>
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
        /// Обновление рабочих значений рабочих переменных
        /// </summary>
        /// <param name="variable">Название переменной</param>
        /// <param name="value">Значение переменной</param>
        public void SetWorkingVariable(string variable, string value)
        {
            botDataBase.Open();
            try
            {
                command = new SQLiteCommand("UPDATE WorkingVariables SET value='" + value + "' WHERE variable='" + variable + "';", botDataBase);
                _ = command.ExecuteNonQuery();
            }
            catch
            {
                botDataBase.Close();
            }
            botDataBase.Close();
        }
        /// <summary>
        /// Получение рабочих значений рабочих переменных
        /// </summary>
        /// <param name="variable">Название переменной</param>
        /// <returns>Значение переменной</returns>
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
        /// Очистка таблицы от всех элементов
        /// </summary>
        /// <param name="table">название таблицы</param>
        public void CleanTable(string table)
        {
            botDataBase.Open();
            command = new SQLiteCommand("DELETE FROM " + table, botDataBase);
            _ = command.ExecuteNonQuery();
            botDataBase.Close();
        }
        /// <summary>
        /// Получение списка строк из таблицы
        /// </summary>
        /// <param name="table">Название таблицы из которой нужно извлечь данные</param>
        /// <param name="column">Столбец таблицы из которого нужно извлечь данные</param>
        /// <returns></returns>
        public List<string> GetListString(string table, int column = 0, string condition = "")
        {
            List<string> list = new();
            botDataBase.Open();
            command = new SQLiteCommand("SELECT * FROM " + table + " " + condition, botDataBase);
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
        /// Получение списка по типу Ключ - Значение из таблицы БД
        /// </summary>
        /// <param name="table">Имя таблицы</param>
        /// <param name="column0">Номер первого столбца</param>
        /// <param name="column1">Номер второго столбца</param>
        /// <returns></returns>
        public Dictionary<string, string> GetDictionaryString(string table, int column0 = 0, int column1 = 1)
        {
            Dictionary<string, string> pairs = new();
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
        /// Получение числа строк в таблице БД
        /// </summary>
        /// <param name="table">Имя таблицы</param>
        /// <returns>Число строк</returns>
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
        /// Костыльный метод для быстрого завершения голосования за нового опера 
        /// </summary>
        /// <returns>true - если было 3 голоса и они все за одного опера</returns>
        public bool FastFinishVote(string operName)
        {
            botDataBase.Open();
            command = new SQLiteCommand("SELECT MAX(count) FROM (SELECT OperName, COUNT(*) as count FROM Votes WHERE OperName='"+operName+"' GROUP BY OperName ORDER BY count ASC);", botDataBase);
            int maxVotes = Convert.ToInt32(command.ExecuteScalar());
            command = new SQLiteCommand("SELECT count(*) FROM Votes;", botDataBase);
            int nrOfVotes = Convert.ToInt32(command.ExecuteScalar());;
            botDataBase.Close();
            return maxVotes==3 && maxVotes==nrOfVotes;
        }
        /// <summary>
        /// Проверка таблицы на наличие указанного Значения
        /// </summary>
        /// <param name="value">Значение которое требуется найти</param>
        /// <param name="table">Таблица в которой следует искать</param>
        /// <param name="column">Номер столбца в котором следует искать</param>
        /// <returns>True - если искомое значение найдено в таблице
        /// False - если искомое значение не найдено в таблице</returns>
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
        /// Добавление голоса за опера
        /// </summary>
        /// <param name="UserID">Идентификатор пользователя</param>
        /// <param name="operName">Имя опера, за которого голосовал пользователь</param>
        public void AddVote(long UserID, string operName)
        {
            botDataBase.Open();
            command = new SQLiteCommand("INSERT INTO votes (UserID,OperName) VALUES (" + UserID + ",'" + operName + "');", botDataBase);
            _ = command.ExecuteNonQuery();
            botDataBase.Close();
        }
        /// <summary>
        /// Обновление информации по угрозам
        /// </summary>
        /// <param name="Type">Тип угрозы</param>
        /// <param name="location">Местоположение угрозы</param>
        public void InfoUpdate(string Type, string location)
        {
            botDataBase.Open();
            command = new SQLiteCommand("UPDATE WarningList SET Location='" + location + "' WHERE Type='" + Type + "';", botDataBase);
            _ = command.ExecuteNonQuery();
            botDataBase.Close();
        }
        /// <summary>
        /// Получучение строки из таблицы БД
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
        public string GetMenu(string time, string day)
        {
            string answerCaffeteria = null;
            botDataBase.Open();
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
            command = countRows == 0
                ? new SQLiteCommand("INSERT INTO CaffeteriaMemory (Eat,Time,Day) VALUES ('" + Text + "', '" + time + "', '" + day + "');", botDataBase)
                : new SQLiteCommand("UPDATE CaffeteriaMemory SET Eat='" + Text + "' WHERE Day='" + day + "' AND Time='" + time + "';", botDataBase);
            _ = command.ExecuteNonQuery();
            botDataBase.Close();
        }
        /// <summary>
        /// Получение списка Типов угроз
        /// </summary>
        /// <returns></returns>
        public List<string> GetWarningTypes()
        {
            List<string> list = new();
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
            List<string> list = new();
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
        /// <summary>
        /// Если пользователь ранее не отправлял СПАМ, добавляем его в базу данных, если отправлял даём добро на Кик этого пользователя из беседы
        /// </summary>
        /// <param name="UserID">Идентификатор пользователя</param>
        /// <param name="ChatID">Идентификатор беседы</param>
        /// <returns>true - если пользователя нужно исключить из беседы</returns>
        public bool KickUser(long UserID, long ChatID)
        {
            botDataBase.Open();
            bool response = false;
            command = new SQLiteCommand("SELECT count(*) FROM SPAM WHERE ChatID=" + ChatID + " AND UserID=" + UserID + ";", botDataBase);
            int countRows = Convert.ToInt32(command.ExecuteScalar());
            if (countRows == 0)
            {
                command = new SQLiteCommand("INSERT INTO SPAM (ChatID,UserID) VALUES (" + ChatID + ", " + UserID + ");", botDataBase);
            }
            else
            {
                response = true;
            }

            _ = command.ExecuteNonQuery();
            botDataBase.Close();
            return response;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userID"></param>
        public void UserUsedCommandIncrease(long userID)
        {
            botDataBase.Open();
            command = new SQLiteCommand("SELECT count(*) FROM ComandsFromUser WHERE UserID=" + userID + ";", botDataBase);
            int countRows = Convert.ToInt32(command.ExecuteScalar());
            command = countRows == 0
                ? new SQLiteCommand("INSERT INTO ComandsFromUser (UserID,Commands) VALUES (" + userID + ",1);", botDataBase)
                : new SQLiteCommand("UPDATE ComandsFromUser SET Commands=Commands+1 WHERE UserID=" + userID + ";", botDataBase);
            _ = command.ExecuteNonQuery();
            botDataBase.Close();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public int NrOfCommandsFromUser(long UserID)
        {
            int UsedCommands = -1;
            botDataBase.Open();
            command = new SQLiteCommand("SELECT * FROM ComandsFromUser WHERE UserID='" + UserID + "';", botDataBase);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                try
                {
                    UsedCommands = reader.GetInt32(1);
                }
                catch
                {
                    UsedCommands = -1;
                }
            }
            reader.Close();
            botDataBase.Close();
            return UsedCommands;
        }

        public void SaveFuckingChat(long UserID, bool IsAdmin)
        {
            botDataBase.Open();
            try
            {
                command = new SQLiteCommand("INSERT INTO Fucking_chat (userID,IsAdmin) VALUES (" + UserID + ",'" + IsAdmin.ToString() + "');", botDataBase);
                _ = command.ExecuteNonQuery();
            }
            catch
            {
                botDataBase.Close();
            }
            botDataBase.Close();
        }

        /// <summary>
        /// Получучение числа из таблицы БД
        /// </summary>
        /// <param name="table">Название таблицы</param>
        /// <param name="column">Название столбца</param>
        /// <param name="cell">Данные в одной из ячеек из таблицы</param>
        /// <param name="NrOfColumn">Номер номер столбца из которого стоит извлечь данные</param>
        /// <returns></returns>
        public long GetLong(string table, string column, string cell, int NrOfColumn)
        {
            botDataBase.Open();
            long number = 0;
            command = new SQLiteCommand("SELECT * FROM " + table + " WHERE " + column + "='" + cell + "';", botDataBase);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                number = reader.GetInt64(NrOfColumn);
            }
            reader.Close();
            botDataBase.Close();
            return number;
        }
    }
}
