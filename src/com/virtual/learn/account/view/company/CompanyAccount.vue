<template>
    <div>
        <h1>Entreprise</h1>
        <div id="agencyAuth">
            <auth ref="authMarkdown" :baseModel="baseModel.auth" :readOnly="readOnly"></auth>
        </div>
        <div v-if="readOnly === false">
            <input type="text" name="name" v-model="baseModel.model.name" placeholder="Name" v-on:blur="validateName()" />
            <p v-if="errorLabels.nameError.length > 0" class="error">{{errorLabels.nameError}}</p><br v-else/>
            <input type="text" name="siret" v-model="baseModel.model.siret" placeholder="Siret" v-on:blur="validateSiret()" />
            <p v-if="errorLabels.siretError.length > 0" class="error">{{errorLabels.siretError}}</p><br v-else/>
            <input type="text" name="sector" v-model="baseModel.model.sector" placeholder="Sector" />
        </div>

        <div v-if="readOnly === true">
            <p>{{baseModel.login}}</p>
            <p>{{baseModel.model.name}}</p>
            <p>{{baseModel.model.siret}}</p>
            <p>{{baseModel.model.sector}}</p>
        </div>
        <div id="companyContact">
            <div><contact ref="contactMarkdown" :baseModel="baseModel.model.contact" :readOnly="readOnly"></contact></div>
        </div>
        <div id="companyAdress">
            <div><adress ref="adressMarkdown" :baseModel="baseModel.model.adress" :readOnly="readOnly"></adress></div>
        </div>
        <div id="companyResponsible">
            <div><responsible ref="responsibleMarkdown" :baseModel="baseModel.model.responsible" :readOnly="readOnly"></responsible></div>
        </div>
        <div id="companyFund">
            <div><fund ref="fundMarkdown" :baseModel="baseModel.model.fund" :readOnly="readOnly"></fund></div>
        </div>
        <div>Modules à venir</div>
        <div>Partners à venir</div>
    </div>
</template>

<script>
    import adress from '../Adress.vue'
    import contact from '../Contact.vue'
    import responsible from '../Responsible.vue'
    import fund from './Fund.vue'
    import auth from '../Auth.vue'

    // Only numbers regexp
    var regExpNumber = /^[0-9]+$/;

    export default {
        name: 'CompanyAccount',
        data() {
            return {
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
        methods: {
            validateForm() {
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
                if (!this.$refs.fundMarkdown.validateForm()) {
                    valid = false;
                }
                return valid;
            },
            validateSiret() {
                if (typeof this.baseModel.model.siret == "undefined" || this.baseModel.model.siret.length == 0) {
                    this.errorLabels.siretError = this.errorMessages.siret.empty;
                    return false;
                } else if (this.baseModel.model.siret.length < this.siretSize) {
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
                if (typeof this.baseModel.model.name == "undefined" || this.baseModel.model.name.length == 0) {
                    this.errorLabels.nameError = this.errorMessages.name.empty;
                    return false;
                } else {
                    this.errorLabels.nameError = "";
                }
                return true;
            }
        },
        components: {
            auth: auth,
            adress: adress,
            contact: contact,
            responsible: responsible,
            fund: fund
        },
        props: ['baseModel', 'readOnly']
    }
</script>

<style >

</style>