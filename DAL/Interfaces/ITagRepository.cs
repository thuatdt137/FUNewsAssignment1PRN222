using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface ITagRepository
    {
        IEnumerable<Tag> GetAllTags();
        Tag GetTagById(int tagId);
        void AddTag(Tag tag);
        void UpdateTag(Tag tag);
        void DeleteTag(int tagId);
        void Save();
    }

}
