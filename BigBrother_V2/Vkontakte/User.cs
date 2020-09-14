using VkNet;
using VkNet.Enums.Filters;
using VkNet.Enums.SafetyEnums;
using VkNet.Model.RequestParams;

namespace BigBrother_V2.Vkontakte
{
    class User
    {
        /// <summary>
        /// 
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FirstNameGen { get; set; }
        /// <summary>
        /// 
        /// </summary>C:\Users\maxim\source\repos\BigBrother_V2\BigBrother_V2\Vkontakte\User.cs
        public string LastNameGen { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public VkNet.Enums.Sex Sex { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Domain { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsAdmin { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FullName { get; set; }

        public User(long userID, VkApi BigBrotherClient)
        {
            if (userID > 0)
            {
                var users = BigBrotherClient.Users.Get(new long[] { userID }, ProfileFields.FirstName | ProfileFields.LastName | ProfileFields.Sex | ProfileFields.Domain);
                foreach (var field in users)
                {
                    FirstName = field.FirstName;
                    LastName = field.LastName;
                    Sex = field.Sex;
                    Domain = field.Domain;
                    Id = userID;
                }

                users = BigBrotherClient.Users.Get(new long[] { userID }, ProfileFields.FirstName | ProfileFields.LastName, NameCase.Gen);
                foreach (var user in users)
                {
                    FirstNameGen = user.FirstName;
                    LastNameGen = user.LastName;
                }
#if !DEBUG
                var admins = BigBrotherClient.Groups.GetMembers(new GroupsGetMembersParams { Filter = GroupsMemberFilters.Managers, GroupId = "187905748", });
#else
                var admins = BigBrotherClient.Groups.GetMembers(new GroupsGetMembersParams { Filter = GroupsMemberFilters.Managers, GroupId = "192662250", });
#endif
                IsAdmin = false;
                foreach (var admin in admins)
                {
                    if (admin.Id == userID) IsAdmin = true;
                }
                FullName = FirstName + " " + LastName;
            }
            else
            {
                userID *= -1;
                var groups = BigBrotherClient.Groups.GetById(new[] { "" }, userID.ToString(), GroupsFields.All);
                foreach (var group in groups)
                {
                    FullName = group.Name;
                }
            }
        }
    }
}
