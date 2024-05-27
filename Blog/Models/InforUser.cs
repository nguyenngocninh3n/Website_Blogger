using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;

namespace Blog.Models
{
    public class Dates
    {
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }

    public class DateTimes
    {
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public int Second { get; set; }
    }
    public class InforUser
    {
        //public string IDUser { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        //public string Password { get; set; }
        public string Sex { get; set; }
        public string Email { get; set; }
        public System.DateTime Birthday { get; set; }
        public string Addr { get; set; }
        public string Degree { get; set; }
        public string Job { get; set; }
        public string Facebook { get; set; }
        public string Instagram { get; set; }
        public string Youtube { get; set; }
        public string LinkedIn { get; set; }
        public string Avatar { get; set; }
            


        public bool SexMode { get; set; }
        public bool EmailMode { get; set; }
        public bool BirthdayMode { get; set; }
        public bool AddressMode { get; set; }
        public bool DegreeMode { get; set; }
        public bool JobMode { get; set; }
        public bool FacebookMode { get; set; }
        public bool InstagramMode { get; set; }
        public bool YoutubeMode { get; set; }
        public bool LinkedInMode { get; set; }


        public InforUser(string userName, string sex, string email, System.DateTime birthday, string address, string degree, string job, string facebook, string instagram, string youtube, string linkedIn, bool sexMode, bool emailMode, bool birthdayMode, bool addressMode, bool degreeMode, bool jobMode, bool facebookMode, bool instagramMode, bool youtubeMode, bool linkedInMode, string avatar)
        {
            UserName = userName;
            Sex = sex;
            Email = email;
            Birthday = birthday;
            Addr = address;
            Degree = degree;
            Job = job;
            Facebook = facebook;
            Instagram = instagram;
            Youtube = youtube;
            LinkedIn = linkedIn;
            Avatar = avatar;

            SexMode = sexMode;
            EmailMode = emailMode;
            BirthdayMode = birthdayMode;
            AddressMode = addressMode;
            DegreeMode = degreeMode;
            JobMode = jobMode;
            FacebookMode = facebookMode;
            InstagramMode = instagramMode;
            YoutubeMode = youtubeMode;
            LinkedInMode = linkedInMode;          
        }

        public InforUser()
        {

        }

        public InforUser(User user, User_mode usermode)
        {
            UserName = user.UserName;
            Sex = user.Sex;
            Email = user.Email;
           
           Birthday = Convert.ToDateTime(user.Birthday);
      
            Addr = user.Address;
            Degree = user.Degree;
            Job = user.Job;
            Facebook = user.Facebook;
            Instagram = user.Instagram;
            Youtube = user.Youtube;
            LinkedIn = user.LinkedIn;
            Avatar = user.Avatar;

            SexMode = usermode.SexMode;
            EmailMode = usermode.EmailMode;
            BirthdayMode = usermode.BirthdayMode;
            AddressMode = usermode.AddressMode;
            DegreeMode = usermode.DegreeMode;
            JobMode = usermode.JobMode;
            FacebookMode = usermode.FacebookMode;
            InstagramMode = usermode.InstagramMode;
            YoutubeMode = usermode.YoutubeMode;
            LinkedInMode = usermode.LinkedInMode;
        }

        public InforUser(User user, User_mode usermode, string show)
        {
            UserName = user.UserName;          

            if (usermode.SexMode == true)
            {
                Sex = user.Sex;
            }
            if (usermode.EmailMode == true)
            {
                Email = user.Email;
            }
            if (usermode.BirthdayMode == true)
            {
                Birthday = Convert.ToDateTime(user.Birthday);           
            }
            if (usermode.AddressMode == true)
            {
                Addr = user.Address;              
            }
            if (usermode.DegreeMode == true)
            {
                Degree = user.Degree;
            }
            if (usermode.JobMode == true)
            {
                Job = user.Job;
            }
            if (usermode.FacebookMode == true)
            {
                Facebook = user.Facebook;
            }
            if (usermode.InstagramMode == true)
            {
                Instagram = user.Instagram;
            }
            if (usermode.YoutubeMode == true)
            {
                Youtube = user.Youtube;
            }
            if (usermode.LinkedInMode == true)
            {
                LinkedIn = user.LinkedIn;
            }
            Avatar = user.Avatar;

            SexMode = usermode.SexMode;
            EmailMode = usermode.EmailMode;
            BirthdayMode = usermode.BirthdayMode;
            AddressMode = usermode.AddressMode;
            DegreeMode = usermode.DegreeMode;
            JobMode = usermode.JobMode;
            FacebookMode = usermode.FacebookMode;
            InstagramMode = usermode.InstagramMode;
            YoutubeMode = usermode.YoutubeMode;
            LinkedInMode = usermode.LinkedInMode;
        }
    }
}