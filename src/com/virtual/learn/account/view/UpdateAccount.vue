<template>
    <div id="page">
        <div id="content">
            <div class="form">
                <b-collapse id="agency" accordion="account-type" class="mt-2">
                    <agencyAccount ref="agency" :baseModel="this.account" :readOnly="false"></agencyAccount>
                </b-collapse>
                <b-collapse id="company" accordion="account-type" class="mt-2">
                    <companyAccount ref="company" :baseModel="this.account" :readOnly="false"></companyAccount>
                </b-collapse>
                <b-collapse id="candidate" accordion="account-type" class="mt-2">
                    <candidateAccount ref="candidate" :baseModel="this.account" :readOnly="false"></candidateAccount>
                </b-collapse>
            </div>
        </div>

        <div id="buttons" class="bloc-buttons">
            <a class="button downlight-button" v-on:click="cancel()">Annuler</a>
            <a class="button highlight-button" v-on:click="update()">Enregistrer</a>
        </div>
    </div>
</template>

<script>
    import agencyAccount from './agency/AgencyAccount.vue'
    import candidateAccount from './candidate/CandidateAccount.vue'
    import companyAccount from './company/CompanyAccount.vue'
    import axios from 'axios'
    export default {
        name: 'CreateAccount',
        data() {
            return {
                account: {
                    auth: {
                        login: "",
                        password: ""
                    },
                    model: {
                        userId: "",
                        name: "",
                        siret: "",
                        sector: "",
                        adress: {
                            number: "",
                            label: "",
                            complement1: "",
                            complement2: "",
                            postalCode: "",
                            town: "",
                            country: ""
                        },
                        contact: {
                            mail: "",
                            phone: "",
                            web: ""
                        },
                        responsible: {
                            firstName: "",
                            lastName: "",
                            position: ""
                        },
                        disability: {},
                        fund: {
                            adress: {}
                        },
                        mobility: {}
                    }
                },
                accountType: ""
            };
        },
        created() {
            var id = this.$route.params.id;
            var instance = this;
            if (typeof id == "undefined" || id != "") {
                axios({
                    method: 'get',
                    url: "/Account/GetAccount"
                }).then(function(response) {
                    instance.accountType = response.data.accountType;
                    instance.account.model = response.data.account;
                    instance.account.auth = response.data.auth;
                    instance.$root.$emit('bv::toggle::collapse', response.data.accountType);
                }).catch(function (error) {
                    console.log(error);
                });
            }
        },
        methods: {
            cancel() {
                // var route = 'read' + this.accountType.charAt(0).toUpperCase() + this.accountType.slice(1) + "Account";
                // var route = "readAccount";
                // this.$router.go({name: route});
                this.$router.go(-1);
            },
            update() {
                var valid = this.validateForm();
                if (valid) {
                    var url = "";
                    if ("agency" == this.accountType) {
                        url = "/Account/UpdateAgencyAccount";
                    } else if ("company" == this.accountType) {
                        url = "/Account/UpdateCompanyAccount";
                    } else if ("candidate" == this.accountType) {
                        url = "/Account/UpdateCandidateAccount";
                    }
                    this.account.accountType = this.accountType;

                    // mandatory
                    // the instance that 'this' reference in axios doesn't point to the vue instance
                    var instance = this;
                    axios({
                        method: 'put',
                        url: url,
                        data: this.account
                    }).then(function(response) {
                        instance.$router.push("/readAccount");
                    }).catch(function (error) {
                        console.log(error);
                    });
                }
            },
            validateForm() {
                if ("agency" == this.accountType) {
                    return this.$refs.agency.validateForm();
                } else if ("company" == this.accountType) {
                    return this.$refs.company.validateForm();
                } else if ("candidate" == this.accountType) {
                    return this.$refs.candidate.validateForm();
                }

            }
        },
        components: {
            'agencyAccount': agencyAccount,
            'candidateAccount': candidateAccount,
            'companyAccount': companyAccount
        }
    }
</script>

<style scoped>
    /* #content, .form {
        background-color: #FFFFFF;
        border: 1px solid #CCCCCC;
        padding: 20px;
        margin-top: 10px;
    } */
</style>