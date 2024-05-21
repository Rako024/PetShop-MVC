using Business.Exceptions;
using Business.Services.Abstracts;
using Core.Models;
using Core.RepositoryAbstracts;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Concretes
{
    public class TeamService : ITeamService
    {
        ITeamRepository _teamRepository;
        IWebHostEnvironment _webHostEnvironment;

        public TeamService(ITeamRepository teamRepository, IWebHostEnvironment webHostEnvironment)
        {
            _teamRepository = teamRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public void CreateTeam(Team team)
        {
            if(team == null)
            {
                throw new NotFoundTeamException("", "Team is not found!");
            }
            if(team.PhotoFile == null)
            {
                throw new NotFoundPhotoFileException("PhotoFile", "PhotoFile is Not Found!");
            }
            if (!team.PhotoFile.ContentType.Contains("image/"))
            {
                throw new DataTypeException("PhotoFile", "Content type is not image!");
            }
            string path = _webHostEnvironment.WebRootPath + @"\upload\team\" + team.PhotoFile.FileName;
            using(FileStream file = new FileStream(path, FileMode.Create))
            {
                team.PhotoFile.CopyTo(file);
            }
            team.ImgUrl = team.PhotoFile.FileName;
            _teamRepository.Add(team);
            _teamRepository.Commit();
        }

        public void DeleteTeam(int id)
        {
            Team team = _teamRepository.Get(x=>x.Id == id);
            if (team == null)
            {
                throw new NotFoundTeamException("", "Team is not found!");
            }
            string existPhotoPath = _webHostEnvironment.WebRootPath + @"\upload\team\" + team.ImgUrl;
            FileInfo existFile = new FileInfo(existPhotoPath);
            existFile.Delete();
            _teamRepository.Delete(team);
            _teamRepository.Commit();
        }

        public List<Team> GetAllTeams(Func<Team, bool>? func = null)
        {
            return _teamRepository.GetAll(func);
        }

        public Team GetTeam(Func<Team, bool>? func = null)
        {
            return _teamRepository.Get(func);
        }

        public void UpdateTeam(int id, Team newTeam)
        {
            Team oldTeam = _teamRepository.Get(x => x.Id == id);
            if (oldTeam == null)
            {
                throw new NotFoundTeamException("", "Team is not found!");
            }
            if(newTeam == null)
            {
                throw new NotFoundTeamException("", "Team is not found!");
            }
            if(newTeam.PhotoFile != null)
            {
                if (!newTeam.PhotoFile.ContentType.Contains("image/"))
                {
                    throw new DataTypeException("PhotoFile", "Content type is not image!");
                }
                string existPhotoPath = _webHostEnvironment.WebRootPath + @"\upload\team\" + oldTeam.ImgUrl;
                FileInfo existFile = new FileInfo(existPhotoPath);
                existFile.Delete();

                string path = _webHostEnvironment.WebRootPath + @"\upload\team\" + newTeam.PhotoFile.FileName;
                using (FileStream file = new FileStream(path, FileMode.Create))
                {
                    newTeam.PhotoFile.CopyTo(file);
                }
                oldTeam.ImgUrl = newTeam.PhotoFile.FileName;
            }
            oldTeam.FullName = newTeam.FullName;
            oldTeam.Designation = newTeam.Designation;
            _teamRepository.Commit();
        }
    }
}
