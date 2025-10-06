using System;
using System.Collections.Generic;
using LibraryManager.Models;

namespace LibraryManager.Services
{
    public interface IMemberService
    {
        List<Member> GetAllMembers();
        Member GetMemberById(int id);
        List<Member> GetActiveMembers();
        bool AddMember(Member member);
        bool UpdateMember(Member member);
        bool DeactivateMember(int id);
    }

    public class MemberService : IMemberService
    {
        private List<Member> _members;

        public MemberService()
        {
            _members = new List<Member>();
            InitializeSampleData();
        }

        private void InitializeSampleData()
        {
            _members.Add(new Member(1, "Jean", "Dupont", "jean.dupont@email.com", "0612345678",
                MembershipType.Premium, DateTime.Now.AddMonths(-12), true));
            
            _members.Add(new Member(2, "Marie", "Martin", "marie.martin@email.com", "0623456789",
                MembershipType.Student, DateTime.Now.AddMonths(-6), true));
            
            _members.Add(new Member(3, "Pierre", "Bernard", "pierre.bernard@email.com", "0634567890",
                MembershipType.Basic, DateTime.Now.AddMonths(-3), true));
        }

        public List<Member> GetAllMembers()
        {
            return new List<Member>(_members);
        }

        public Member GetMemberById(int id)
        {
            foreach (var member in _members)
            {
                if (member.Id == id)
                    return member;
            }
            return null;
        }

        public List<Member> GetActiveMembers()
        {
            List<Member> results = new List<Member>();
            foreach (var member in _members)
            {
                if (member.IsActive)
                    results.Add(member);
            }
            return results;
        }

        public bool AddMember(Member member)
        {
            if (member == null)
                return false;

            int maxId = 0;
            foreach (var m in _members)
            {
                if (m.Id > maxId)
                    maxId = m.Id;
            }
            member.Id = maxId + 1;

            _members.Add(member);
            return true;
        }

        public bool UpdateMember(Member member)
        {
            if (member == null)
                return false;

            for (int i = 0; i < _members.Count; i++)
            {
                if (_members[i].Id == member.Id)
                {
                    _members[i] = member;
                    return true;
                }
            }
            return false;
        }

        public bool DeactivateMember(int id)
        {
            Member member = GetMemberById(id);
            if (member == null)
                return false;

            member.IsActive = false;
            return true;
        }
    }
}
