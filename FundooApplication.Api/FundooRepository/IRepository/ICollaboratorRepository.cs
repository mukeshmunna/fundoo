﻿using FundooModel.Notes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FundooRepository.IRepository
{
    public interface ICollaboratorRepository
    {
        public Task<int> AddCollaborator(Collaborator collaborator);
        public bool DeleteCollab(int noteId, int userId);
        public IEnumerable<Collaborator> GetAllCollabNotes(int userId, string labelId);

        public IEnumerable<NotesCollab> GetAllNotesColllab(int userId);
    }
}
