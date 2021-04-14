﻿using BlogLab.Models.Photo;
using BlogLab.Repository.Abstract;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BlogLab.Repository.Concrete
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly IConfiguration _config;

        public PhotoRepository(IConfiguration config)
        {
            _config = config;
        }

        public async Task<int> DeleteAsync(int photoId)
        {
            int affectedRows = 0;

            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                affectedRows = await connection.ExecuteAsync(
                        "Photo_Delete",
                        new { PhotoId = photoId },
                        commandType: System.Data.CommandType.StoredProcedure
                    );
            }

            return affectedRows;
        }

        public async Task<List<Photo>> GetAllByUserIdAsync(int applicationUserId)
        {
            IEnumerable<Photo> photos;
            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                photos = await connection.QueryAsync<Photo>(
                        "Photo_GetByUserId",
                        new { ApplicationUserId = applicationUserId },
                        commandType: System.Data.CommandType.StoredProcedure
                    );
            }
            return photos.ToList();
        }

        public async Task<Photo> GetAsync(int photoId)
        {
            Photo photo;
            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                photo = await connection.QueryFirstOrDefaultAsync<Photo>(
                        "Photo_GetByUserId",
                        new { PhotoId = photoId },
                        commandType: System.Data.CommandType.StoredProcedure
                    );
            }
            return photo;
        }

        public async Task<Photo> InsertAsync(PhotoCreate photoCreate, int applicationUserId)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("PublicId", typeof(string));
            dataTable.Columns.Add("ImageUrl", typeof(string));
            dataTable.Columns.Add("Description", typeof(string));

            dataTable.Rows.Add(photoCreate.PublicId, photoCreate.ImageUrl, photoCreate.Description);

            int newPhotoId;

            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                newPhotoId = await connection.ExecuteScalarAsync<int>(
                        "Photo_Insert",
                        new
                        {
                            Photo = dataTable.AsTableValuedParameter("dbo.PhotoType"),
                            ApplicationUserId = applicationUserId
                        },
                        commandType: System.Data.CommandType.StoredProcedure
                    );
            }
            var photo = await GetAsync(newPhotoId);
            return photo;
        }
    }
}