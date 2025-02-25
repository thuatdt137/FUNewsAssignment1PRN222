using DAL.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly FunewsManagementContext _context;

        public TagRepository(FunewsManagementContext context)
        {
            _context = context;
        }

        public IEnumerable<Tag> GetAllTags()
        {
            return _context.Tags.ToList();
        }

        public Tag GetTagById(int tagId)
        {
            return _context.Tags.Find(tagId);
        }

        public void AddTag(Tag tag)
        {
            _context.Tags.Add(tag);
        }

        public void UpdateTag(Tag tag)
        {
            _context.Tags.Update(tag);
        }

        public void DeleteTag(int tagId)
        {
            var tag = _context.Tags.Find(tagId);
            if (tag != null)
            {
                _context.Tags.Remove(tag);
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }

}
