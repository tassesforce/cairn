using System.Collections.Generic;
using cairn.Accounts.Collaborator;

namespace cairn.Models.Accounts
{ 
    /// <summary>Resource for an agency account (moral)</summary>
    public partial class AgencyAccountModelWithCollaborator : AgencyAccountModel 
    { 

        /// <summary>List of the collaborators of the account</summary>
        public List<CollaboratorAccountModel> Collaborators {get; set;}

        /// <summary>Default constructor</summary>
        public AgencyAccountModelWithCollaborator() : base()
        {

        }

        /// <summary>Constructor used for cloning</summary>
        public AgencyAccountModelWithCollaborator(AgencyAccountModel baseAccount)
        {
            this.Adress = baseAccount.Adress;
            this.Contact = baseAccount.Contact;
            this.Name = baseAccount.Name;
            this.Partners = baseAccount.Partners;
            this.Responsible = baseAccount.Responsible;
            this.Sector = baseAccount.Sector;
            this.Siret = baseAccount.Siret;
            this.UserId = baseAccount.UserId;
        }

    }
}