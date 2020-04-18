
using SchoolManager.Resources;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SchoolManager.Domain.Entities
{
    [MetadataType(typeof(RegistrationRequestValidation))]
    public partial class RegistrationRequest
    {
        private const string cFaltanDatos = "Faltan datos";
        private const string cAprobada = "Aprobada";
        private const string cRechazada = "Rechazada";
        private const string cAcontrolar = "A controlar";

        [NotMapped]
        public bool OtherSchoolNoNullExclusive
        {
            get { return OtherSchool ?? false; }
            set { OtherSchool = value; }
        }
        [NotMapped]
        public bool HealthCompletedNoNullExclusive
        {
            get { return HealthCompleted ?? false; }
            set { HealthCompleted = value; }
        }
        [NotMapped]
        public bool HealthUpdatedNoNullExclusive
        {
            get { return HealthUpdated ?? false; }
            set { HealthUpdated = value; }
        }
        [NotMapped]
        public bool PrivateNoNullExclusive
        {
            get { return Private ?? false; }
            set { Private = value; }
        }

        public bool AControlar()
        {
            return Status.Equals(cAcontrolar);
        }

        public bool Aprobada()
        {
            return Status.Equals(cAprobada);
        }

        public bool FaltanDatos()
        {
            return Status.Equals(cFaltanDatos);
        }

        public void setFaltanDatos()
        {
            Status = cFaltanDatos;
        }

        public void setCompleto()
        {
            Status = cAcontrolar;
        }

        public void setAprobada()
        {
            Status = cAprobada;
        }

        public bool EstaCompleta()
        {
            return this.DatosInscripcionOK() && this.DatosAlumnoOK() && this.DatosMadreOK() && this.DatosPadreOK() && this.DatosTutorOK();
        }

        private bool DatosTutorOK()
        {
            return (this.FathersLive.HasValue && this.FathersLive.Value) ||
                (this.MothersLive.HasValue && this.MothersLive.Value) ||
                (this.MothersLive.HasValue && !this.MothersLive.Value && this.FathersLive.HasValue && !this.FathersLive.Value &&
                    !string.IsNullOrEmpty(this.TutorName) &&
                    this.TutorLive.HasValue &&
                    ((this.TutorLive.Value &&
                        !string.IsNullOrEmpty(this.TutorKindOfDocument) &&
                        this.TutorDocumentNumber.HasValue &&
                        !string.IsNullOrEmpty(this.TutorAddressStreet) &&
                        !string.IsNullOrEmpty(this.TutorAddressNumber) &&
                        !string.IsNullOrEmpty(this.TutorPhone)
                        ) || (!this.TutorLive.Value))
                );
        }

        private bool DatosPadreOK()
        {
            return !string.IsNullOrEmpty(this.FathersName) &&
                this.FathersLive.HasValue &&
                ((this.FathersLive.Value &&
                    !string.IsNullOrEmpty(this.FathersKindofDocument) &&
                    this.FathersDocumentNumber.HasValue &&
                    !string.IsNullOrEmpty(this.FathersAddressStreet) &&
                    !string.IsNullOrEmpty(this.FathersAddressNumber) &&
                    !string.IsNullOrEmpty(this.FathersPhone)
                    ) || (!this.FathersLive.Value))
                ;
        }

        private bool DatosMadreOK()
        {
            return !string.IsNullOrEmpty(this.MothersName) &&
                this.MothersLive.HasValue &&
                ((this.MothersLive.Value &&
                    !string.IsNullOrEmpty(this.MothersKindOfDocument) &&
                    this.MothersDocumentNumber.HasValue &&
                    !string.IsNullOrEmpty(this.MothersAddressStreet) &&
                    !string.IsNullOrEmpty(this.MothersAddressNumber) &&
                    !string.IsNullOrEmpty(this.MothersPhone)
                    ) || (!this.MothersLive.Value))
                ;
        }

        private bool DatosAlumnoOK()
        {
            return !string.IsNullOrEmpty(this.Lastnames) &&
                !string.IsNullOrEmpty(this.Firstnames) &&
                this.Birthday.HasValue &&
                !string.IsNullOrEmpty(this.Birthplace) &&
                !string.IsNullOrEmpty(this.DNI) &&
                !string.IsNullOrEmpty(this.Sex) &&
                !string.IsNullOrEmpty(this.AdressStreet) &&
                this.AdressNumber.HasValue &&
                !string.IsNullOrEmpty(this.Localidad) &&
                (!string.IsNullOrEmpty(this.CellPhone) || !string.IsNullOrEmpty(this.FamilyPhone))
                ;
            
            
        }
        private bool DatosInscripcionOK()
        {
            return !string.IsNullOrEmpty(this.Level) &&
                !string.IsNullOrEmpty(this.Year) &&
                !string.IsNullOrEmpty(this.Turn);
        }


        public void setRechazada()
        {
            Status = cRechazada;
        }

        public bool EstaAprobada()
        {
            return Status.Equals(cAprobada);
        }

        public bool EstaRechazada()
        {
            return Status.Equals(cRechazada);
        }
    }
    public class RegistrationRequestValidation
    {
        //[Required(ErrorMessageResourceName = "MEN004", ErrorMessageResourceType = typeof(Messages))]
        [DisplayName("NIVEL/RAMA")]
        public string Level { get; set; }
        [DisplayName("AÑO")]
        public string Year { get; set; }
        [DisplayName("TURNO SOLICITADO")]
        public string Turn { get; set; }
        [DisplayName("Apellido(s)")]
        public string Lastnames { get; set; }
        [DisplayName("Nombre(s)")]
        public string Firstnames { get; set; }
        [DisplayName("Fecha de nacimiento")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> Birthday { get; set; }
        [DisplayName("Lugar de nacimiento")]
        public string Birthplace { get; set; }
        [DisplayName("D.N.I.")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "MEN004", ErrorMessageResourceType = typeof(Messages))]
        public string DNI { get; set; }
        [DisplayName("Sexo")]
        public string Sex { get; set; }
        [DisplayName("Calle")]
        public string AdressStreet { get; set; }
        [DisplayName("N°")]
        public Nullable<int> AdressNumber { get; set; }
        [DisplayName("Piso")]
        public string AdressFloor { get; set; }
        [DisplayName("Dpto")]
        public string AdressDpto { get; set; }
        [DisplayName("Localidad")]
        public string Localidad { get; set; }
        [DisplayName("Código Postal")]
        public Nullable<int> PostalCode { get; set; }
        [DisplayName("Teléfono familiar")]
        public string FamilyPhone { get; set; }
        [DisplayName("Celular")]
        public string CellPhone { get; set; }
        [DisplayName("Otros teléfonos (abuela, etc.)")]
        public string OtherPhones { get; set; }
        [DisplayName("Dirección de e-mail del padre")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Email Inválido")]
        public string EmailFather { get; set; }
        [DisplayName("Dirección de e-mail de la madre")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Email Inválido")]
        public string EmailMother { get; set; }
        [DisplayName("¿Cuántos hermanos tiene?")]
        public Nullable<byte> HMB { get; set; }
        [DisplayName("¿Cuántos en este establecimiento?")]        
        public Nullable<byte> HMITS { get; set; }
        [DisplayName("Fecha de nacimiento de los hermanos que quiere dejar inscriptos")]
        public string BDB { get; set; }
        [DisplayName("Si no habla español, lengua hablada en el hogar")]                        
        public string Language { get; set; }
        
        public Nullable<bool> OtherSchool { get; set; }
        [DisplayName("Distrito")]         
        public string Distrito { get; set; }
        [DisplayName("Nivel/Rama")]                 
        public string OtherSchoolLevel { get; set; }
        [DisplayName("Nombre del Establecimiento")]   
        public string NameSchool { get; set; }
        [DisplayName("Nro")]  
        public Nullable<long> NumberSchool { get; set; }
        
        public Nullable<bool> Private { get; set; }
        [DisplayName("Obra Social")]                  
        public string SocialName { get; set; }
        [DisplayName("N° Afiliado")]                 
        public string AffiliateSocialNumber { get; set; }
        [DisplayName("Apellido/s y Nombres de la Madre")]                         
        public string MothersName { get; set; }
        [DisplayName("Profesión u ocupación")]
        public string MothersProfession { get; set; }
        [DisplayName("Nacionalidad")]
        public string MothersNationality { get; set; }
        [DisplayName("Nivel de instrucción de la Madre")]
        public string MothersLevelInstruction { get; set; }
        public Nullable<bool> MothersLevelInstructionCompleted { get; set; }
        [DisplayName("Hasta el año/grado")]
        public string MothersUntilYearLevel { get; set; }
        [DisplayName("¿Vive?")]
        public Nullable<bool> MothersLive { get; set; }
        [DisplayName("Tipo de documento")]
        public string MothersKindOfDocument { get; set; }
        [DisplayName("N° Doc.")]
        public Nullable<long> MothersDocumentNumber { get; set; }
        [DisplayName("Calle")]
        public string MothersAddressStreet { get; set; }
        [DisplayName("N°")]
        public string MothersAddressNumber { get; set; }
        [DisplayName("Piso")]
        public string MothersAddressFloor { get; set; }
        [DisplayName("Torre")]
        public string MothersAddressTower { get; set; }
        [DisplayName("Dpto")]
        public string MothersAddressDpto { get; set; }
        [DisplayName("Localidad")]
        public string MothersLocalidad { get; set; }
        [DisplayName("Código Postal")]
        public Nullable<long> MothersPostalCode { get; set; }
        [DisplayName("Teléfono")]
        public string MothersPhone { get; set; }
        [DisplayName("Teléfonos del trabajo")]
        public string MothersWorkPhone { get; set; }
        [DisplayName("Apellido/s y Nombres del Padre")]
        public string FathersName { get; set; }
        [DisplayName("Profesión u ocupación")]
        public string FathersProfession { get; set; }
        [DisplayName("Nacionalidad")]        
        public string FathersNationality { get; set; }
        [DisplayName("Nivel de instrucción del Padre")]
        public string FathersLevelInstruction { get; set; }
        
        public Nullable<bool> FathersLevelInstructionCompleted { get; set; }
        [DisplayName("Hasta el año/grado")]
        public string FathersUntilYearLevel { get; set; }
        [DisplayName("¿Vive?")]
        public Nullable<bool> FathersLive { get; set; }
        [DisplayName("Tipo de documento")]
        public string FathersKindofDocument { get; set; }
        [DisplayName("N° Doc.")]
        public Nullable<long> FathersDocumentNumber { get; set; }
        [DisplayName("Calle")]
        public string FathersAddressStreet { get; set; }
        [DisplayName("N°")]
        public string FathersAddressNumber { get; set; }
        [DisplayName("Piso")]
        public string FathersAddressFloor { get; set; }
        [DisplayName("Torre")]
        public string FathersAddressTower { get; set; }
        [DisplayName("Dpto")]
        public string FathersAddressDpto { get; set; }
        [DisplayName("Localidad")]
        public string FathersLocalidad { get; set; }
        [DisplayName("Código Postal")]
        public Nullable<long> FathersPostalCode { get; set; }
        [DisplayName("Teléfono")]
        public string FathersPhone { get; set; }
        [DisplayName("Teléfonos del trabajo")]
        public string FathersWorkPhone { get; set; }
        [DisplayName("Apellido/s y Nombres del Tutor/responsable")]
        public string TutorName { get; set; }
        [DisplayName("Profesión u ocupación")]
        public string TutorProfession { get; set; }
        [DisplayName("Nacionalidad")]
        public string TutorNationality { get; set; }
        [DisplayName("Nivel de instrucción del Tutor/responsable")]
        public string TutorLevelInstruction { get; set; }
        
        public Nullable<bool> TutorLevelInstructionCompleted { get; set; }
        [DisplayName("Hasta el año/grado")]
        public string TutorUntilYearLevel { get; set; }
        [DisplayName("¿Vive?")]
        public Nullable<bool> TutorLive { get; set; }
        [DisplayName("Tipo de documento")]
        public string TutorKindOfDocument { get; set; }
        [DisplayName("N° Doc.")]
        public Nullable<long> TutorDocumentNumber { get; set; }
        [DisplayName("Calle")]
        public string TutorAddressStreet { get; set; }
        [DisplayName("N°")]
        public string TutorAddressNumber { get; set; }
        [DisplayName("Piso")]
        public string TutorAddressFloor { get; set; }
        [DisplayName("Torre")]
        public string TutorAddressTower { get; set; }
        [DisplayName("Dpto")]
        public string TutorAddressDpto { get; set; }
        [DisplayName("Localidad")]
        public string TutorLocalidad { get; set; }
        [DisplayName("Código Postal")]
        public Nullable<long> TutorPostalCode { get; set; }
        [DisplayName("Teléfono")]
        public string TutorPhone { get; set; }
        [DisplayName("Teléfonos del trabajo")]
        public string TutorWorkPhone { get; set; }
        [DisplayName("Apellido(s) y Nombre(s)")]
        public string OtherPersonsName { get; set; }
        [DisplayName("Tipo doc")]
        public string PersonKindDocument { get; set; }
        [DisplayName("N° Doc")]
        public Nullable<long> PersonDocumentNumber { get; set; }
        [DisplayName("Año lectivo")]
        public int YearRequest { get; set; }
        [DisplayName("Estado")]
        public string Status { get; set; }
        
        public Nullable<bool> HealthCompleted { get; set; }
        
        public Nullable<bool> HealthUpdated { get; set; }
        [DisplayName("Proviene de otro Establecimiento")] 
        public bool OtherSchoolNoNullExclusive
        {
            get { return OtherSchool ?? false; }
            set { OtherSchool = value; }
        }
        [DisplayName("Completó")]
        public bool HealthCompletedNoNullExclusive
        {
            get { return HealthCompleted ?? false; }
            set { HealthCompleted = value; }
        }
        [DisplayName("Actualizó")]
        public bool HealthUpdatedNoNullExclusive
        {
            get { return HealthUpdated ?? false; }
            set { HealthUpdated = value; }
        }
        [DisplayName("Privado")]  
        public bool PrivateNoNullExclusive
        {
            get { return Private ?? false; }
            set { Private = value; }
        }
    }
}
