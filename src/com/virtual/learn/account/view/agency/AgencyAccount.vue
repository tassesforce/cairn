<template>
    <div>
        <h1>Agence</h1>
        <div id="agencyAuth">
            <auth ref="authMarkdown" :baseModel="baseModel.auth" :readOnly="readOnly"></auth>
        </div>
        <div id="updateAgency" v-if="readOnly === false">
            <span class="mandatory">*</span><input type="text" name="name" v-model="baseModel.model.name" placeholder="Name" v-on:blur="validateName()" />
            <p v-if="errorLabels.nameError.length > 0" class="error">{{errorLabels.nameError}}</p><br v-else/>
            <span class="mandatory">*</span><input type="text" name="siret" v-model="baseModel.model.siret" placeholder="Siret" v-on:blur="validateSiret()" />
            <p v-if="errorLabels.siretError.length > 0" class="error">{{errorLabels.siretError}}</p><br v-else/>
            <input type="text" name="sector" v-model="baseModel.model.sector" placeholder="Sector" />
        </div>

        <div id="readAgency" v-if="readOnly === true">
            <p>{{baseModel.model.name}}</p>
            <p>{{baseModel.model.siret}}</p>
            <p>{{baseModel.model.sector}}</p>
        </div>
        <div id="agencyContact">
            <div><contact ref="contactMarkdown" :baseModel="baseModel.model.contact" :readOnly="readOnly"></contact></div>
        </div>
        <div id="agencyAdress">
            <div><adress ref="adressMarkdown" :baseModel="baseModel.model.adress" :readOnly="readOnly"></adress></div>
        </div>
        <div id="agencyResponsible">
            <div><responsible ref="responsibleMarkdown" :baseModel="baseModel.model.responsible" :readOnly="readOnly"></responsible></div>
        </div>
        <br/>
        <div id="collaborators">
            <div class="collaborators" v-for="collab in baseModel.model.collaborators" v-bind:key="collab.id" >
                <div v-on:click="seeCollab(collab.id)">
                    {{collab.firstName}} {{collab.lastName}}<br/>
                    <div v-if="typeof collab.position != 'undefined' && collab.position.length > 0">
                        {{collab.position}}
                    </div>
                    <contact :baseModel="collab.contact" :readOnly="true"></contact>
                </div>
                <img src="../../../images/icons/trash.png" class="icons" v-on:click="deleteCollab(collab.id)"/>
            </div>
        </div>
    </div>
</template>

<script>
    import adress from '../Adress.vue'
    import contact from '../Contact.vue'
    import responsible from '../Responsible.vue'
    import auth from '../Auth.vue'
    import axios from 'axios'

    export default {
        name: 'AgencyAccount',
        data() {
            return {
                siretSize: 14,
                errorLabels: {
                    siretError: "",
                    nameError: ""
                },
                errorMessages: {
                    siret: {
                        empty: "Merci de saisir un numéro SIRET.",
                        size: "Le SIRET doit être composé de 14 caractères.",
                        format: "Le SIRET n'est composé que de numéros."
                    },
                    name: {
                        empty: "Le nom de société est obligatoire."
                    }
                }
            };
        },
        components: {
            'adress': adress,
            'contact': contact,
            'responsible': responsible,
            'auth': auth
        },
        methods: {
            validateForm() {
                // Validate the form, to see if we can perform the final action
                var valid = true;
                if (!this.validateSiret()) {
                    valid = false;
                }
                if (!this.validateName()) {
                    valid = false;
                }
                if (!this.$refs.adressMarkdown.validateForm()) {
                    valid = false;
                }
                if (!this.$refs.contactMarkdown.validateForm()) {
                    valid = false;
                }
                if (!this.$refs.responsibleMarkdown.validateForm()) {
                    valid = false;
                }
                if (!this.$refs.authMarkdown.validateForm()) {
                    valid = false;
                }
                return valid;
            },
            validateSiret() {
                // Validate the SIRET number => only numbers, size of 14 and mandatory
                // Only numbers regexp
                var regExpNumber = /^[0-9]+$/;

                if (typeof this.baseModel.model.siret == "undefined" || this.baseModel.model.siret.length == 0) {
                    this.errorLabels.siretError = this.errorMessages.siret.empty;
                    return false;
                } else if (this.baseModel.model.siret.length != this.siretSize) {
                    this.errorLabels.siretError = this.errorMessages.siret.size;
                    return false;
                } else if (!regExpNumber.test(this.baseModel.model.siret)) {
                    this.errorLabels.siretError = this.errorMessages.siret.format;
                    return false;
                } else {
                    this.errorLabels.siretError = "";
                }
                return true;
            },
            validateName() {
                // Validate the presence of a name for this agency
                if (typeof this.baseModel.model.name == "undefined" || this.baseModel.model.name.length == 0) {
                    this.errorLabels.nameError = this.errorMessages.name.empty;
                    return false;
                } else {
                    this.errorLabels.nameError = "";
                }
                return true;
            },
            seeCollab(idCollab) {
                this.$router.push({ name: 'readEmbeddedCollaboratorAccount', params: { readOnly: true, search: true, id: idCollab, embedded: true} });
            },
            deleteCollab(idCollab) {
                console.log("deleteCollab " + idCollab);
                if (typeof idCollab != 'undefined' && idCollab.length > 0) {
                    // mandatory
                    // the instance that 'this' reference in axios doesn't point to the vue instance
                    var instance = this;
                    var url = "/Account/DeleteCollaboratorAccount?userId=" + idCollab;
                    axios({
                        method: 'delete',
                        url: url
                    }).then(function(response) {
                        console.log("Suppression faite");
                    }).catch(function (error) {
                        console.log(error);
                    });
                } else {
                    // show error is vide (should not happen)
                }
            }
        },
        props: ['baseModel', 'readOnly']
        
    }
</script>

.collaborators {
    border: solid black 1px;
    width: 200px;
    margin: 20px;
    padding: 20px;
}

.icons {
    width: 30px;
}
</style>