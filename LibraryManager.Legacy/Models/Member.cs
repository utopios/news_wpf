using System;

namespace LibraryManager.Models
{
    public enum MembershipType
    {
        Basic,
        Premium,
        Student,
        Senior
    }

    public class Member
    {
        private int _id;
        private string _firstName;
        private string _lastName;
        private string _email;
        private string _phone;
        private MembershipType _membershipType;
        private DateTime _registrationDate;
        private bool _isActive;

        public Member(int id, string firstName, string lastName, string email, 
                     string phone, MembershipType membershipType, 
                     DateTime registrationDate, bool isActive)
        {
            _id = id;
            _firstName = firstName;
            _lastName = lastName;
            _email = email;
            _phone = phone;
            _membershipType = membershipType;
            _registrationDate = registrationDate;
            _isActive = isActive;
        }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public string Phone
        {
            get { return _phone; }
            set { _phone = value; }
        }

        public MembershipType MembershipType
        {
            get { return _membershipType; }
            set { _membershipType = value; }
        }

        public DateTime RegistrationDate
        {
            get { return _registrationDate; }
            set { _registrationDate = value; }
        }

        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }

        public string GetFullName()
        {
            return _firstName + " " + _lastName;
        }

        public string GetMembershipBadge()
        {
            if (_membershipType == MembershipType.Premium)
                return "[PREMIUM] Premium";
            else if (_membershipType == MembershipType.Student)
                return "[ETUDIANT] Étudiant";
            else if (_membershipType == MembershipType.Senior)
                return "[SENIOR] Senior";
            else
                return "[BASIC] Basic";
        }

        public int GetMaxBorrowLimit()
        {
            switch (_membershipType)
            {
                case MembershipType.Premium:
                    return 10;
                case MembershipType.Student:
                    return 5;
                case MembershipType.Senior:
                    return 5;
                case MembershipType.Basic:
                    return 3;
                default:
                    return 3;
            }
        }

        public int GetBorrowDurationDays()
        {
            if (_membershipType == MembershipType.Premium)
                return 30;
            else if (_membershipType == MembershipType.Student)
                return 21;
            else
                return 14;
        }
    }
}
