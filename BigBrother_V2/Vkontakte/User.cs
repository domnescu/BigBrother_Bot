using VkNet;
using VkNet.Enums.Filters;
using VkNet.Enums.StringEnums;
using VkNet.Model;

namespace BigBrother_V2.Vkontakte
{
    /// <summary>
    /// Объект с информацией о пользователе который написал
    /// </summary>
    internal class User
    {
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Фамилия пользователя
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// Имя пользователя в родительном падеже
        /// </summary>
        public string FirstNameGen { get; set; }
        /// <summary>
        /// Фамилия пользователя в родительном падеже
        /// </summary>
        public string LastNameGen { get; set; }
        /// <summary>
        /// Пол пользователя
        /// </summary>
        public VkNet.Enums.Sex Sex { get; set; }
        /// <summary>
        /// Короткая ссылка на страницу пользователя
        /// </summary>
        public string Domain { get; set; }
        /// <summary>
        /// Является ли пользователь Администратором сообщества
        /// </summary>
        public bool IsAdmin { get; set; }
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Имя и Фамилия пользователя
        /// </summary>
        public string FullName { get; set; }

        public User(long userID, VkApi BigBrotherClient)
        {
            if (userID > 0)
            {
                System.Collections.ObjectModel.ReadOnlyCollection<VkNet.Model.User> users = BigBrotherClient.Users.Get(new long[] { userID }, ProfileFields.FirstName | ProfileFields.LastName | ProfileFields.Sex | ProfileFields.Domain);
                foreach (VkNet.Model.User field in users)
                {
                    FirstName = field.FirstName;
                    LastName = field.LastName;
                    Sex = (VkNet.Enums.Sex)field.Sex;
                    Domain = "[id"+userID +"|@" + field.Domain+"]";
                    Id = userID;
                }

                users = BigBrotherClient.Users.Get(new long[] { userID }, ProfileFields.FirstName | ProfileFields.LastName, NameCase.Gen);
                foreach (VkNet.Model.User user in users)
                {
                    FirstNameGen = user.FirstName;
                    LastNameGen = user.LastName;
                }
#if !DEBUG
                var admins = BigBrotherClient.Groups.GetMembers(new GroupsGetMembersParams { Filter = GroupsMemberFilters.Managers, GroupId = "187905748", });
#else
                VkNet.Utils.VkCollection<VkNet.Model.User> admins = BigBrotherClient.Groups.GetMembers(new GroupsGetMembersParams { Filter = GroupsMemberFilters.Managers, GroupId = "192662250", });
#endif
                IsAdmin = false;
                foreach (VkNet.Model.User admin in admins)
                {
                    if (admin.Id == userID)
                    {
                        IsAdmin = true;
                    }
                }
                FullName = FirstName + " " + LastName;
            }
            else
            {
                userID *= -1;
                System.Collections.ObjectModel.ReadOnlyCollection<VkNet.Model.Group> groups = BigBrotherClient.Groups.GetById(new[] { "" }, userID.ToString(), GroupsFields.All);
                foreach (VkNet.Model.Group group in groups)
                {
                    FullName = group.Name;
                }
            }
        }
    }
}
