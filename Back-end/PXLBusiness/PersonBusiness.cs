using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PXLBusinessData;
using System.Data;

namespace PXLBusiness
{
    public class PersonBusiness
    {
        public PersonBusiness()
        {
            if (LoginData.LoginSuccessFul)
            {
                LoginData.Start();
            }
            else
            {
                throw new Exception("Gelieve correct in te loggen!");
            }
        }
        ~PersonBusiness()
        {
            LoginData.Stop();
        }
        public void CreatePerson(PersDto persDto)
        {
            persDto.Creationid = LoginData.UserID;
            persDto.CreationDate = DateTime.Now;
            PersData persData = new PersData(persDto);
            persDto.Personid= persData.CreateRecord();
        }
        public void UpdateUser(PersDto persDto, int primaryKeyValue)
        {
            PersData persData = new PersData(persDto);
            persData.UpdateRecord(primaryKeyValue);
        }
        public PersDto GetUserDto(int primaryKeyValue)
        {
            PersData persData = new PersData();
            return persData.EntityUser(primaryKeyValue);
        }
        public List<PersDto> GetPersDto()
        {
            PersData persData = new PersData();
            return persData.EntityUsers();
        }
        public DataTable GetUserData()
        {
            PersData persData = new PersData();
            DataTable dt = persData.GetRecords();
            //DatabaseLauncher.StopWithoutTransaction();
            return dt;
        }       
    }
}
