using Business.Interfaces;
using DAL.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;

        public TagService(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public IEnumerable<Tag> GetAllTags()
        {
            return _tagRepository.GetAllTags();
        }

        public Tag GetTagById(int tagId)
        {
            return _tagRepository.GetTagById(tagId);
        }

        public void AddTag(Tag tag)
        {
            _tagRepository.AddTag(tag);
            _tagRepository.Save();
        }

        public void UpdateTag(Tag tag)
        {
            _tagRepository.UpdateTag(tag);
            _tagRepository.Save();
        }

        public void DeleteTag(int tagId)
        {
            _tagRepository.DeleteTag(tagId);
            _tagRepository.Save();
        }
    }

}
