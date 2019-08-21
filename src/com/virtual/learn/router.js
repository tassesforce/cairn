import Vue from 'vue'
import Router from 'vue-router'
import LoginComponent from "./auth/view/Login.vue"
import VueResource from 'vue-resource';
import axios from 'axios'
import BootstrapVue from 'bootstrap-vue'
import 'bootstrap/dist/css/bootstrap.css'
import 'bootstrap-vue/dist/bootstrap-vue.css'
import VeeValidate from 'vee-validate';
import VueShortkey from 'vue-shortkey';

import AgencyProfileComponent from "./account/view/agency/AgencyProfile.vue"
import CandidateProfileComponent from "./account/view/candidate/CandidateProfile.vue"
import CompanyProfileComponent from "./account/view/company/CompanyProfile.vue"
import CollaboratorProfileComponent from "./account/view/collaborator/CollaboratorProfile.vue"
import CreateAccountComponent from "./account/view/CreateAccount.vue"
import UpdateAccountComponent from "./account/view/UpdateAccount.vue"
import ReadAccountComponent from "./account/view/ReadAccount.vue"
import CreateCollaboratorAccountComponent from "./account/view/collaborator/CreateCollaboratorAccount.vue"
import CollaboratorAccountComponent from "./account/view/collaborator/CollaboratorAccount.vue"
import CatalogComponent from "./module/view/Catalog.vue"

Vue.use(VeeValidate);
Vue.use(BootstrapVue)
Vue.use(Router)
Vue.use(VueResource);
Vue.use(VueShortkey);

axios.defaults.withCredentials = true;
axios.defaults.headers.common = {
    "Content-Type": "application/x-www-form-urlencoded"
  };
axios.create({
    // withCredentials = true
})

// export default new Router({
const router = new Router({
    routes: [
        {
            path: '/',
            redirect: {
                name: "login"
            }
        },
        {
            path: "/login",
            name: "login",
            component: LoginComponent
        },
        {
            path: "/profile/agency",
            name: "agencyProfile",
            component: AgencyProfileComponent,
            children: [
                {
                    path: "/account/:id",
                    name: "updateAgencyAccount",
                    component: UpdateAccountComponent
                },
                {
                    path: "/readAccount",
                    name: "readAgencyAccount",
                    component: ReadAccountComponent
                },
                {
                    path: "/collaborator/create",
                    name: "createEmbeddedCollaboratorAccount",
                    component: CreateCollaboratorAccountComponent
                },
                {
                    path: "/collaborator/read",
                    name: "readEmbeddedCollaboratorAccount",
                    component: CollaboratorAccountComponent
                },
                {
                    path: "/catalog",
                    name: "catalog",
                    component: CatalogComponent
                }
              ]
        },
        {
            path: "/profile/company",
            name: "companyProfile",
            component: CompanyProfileComponent,
            children: [
                {
                    path: "/account/:id",
                    name: "updateCompanyAccount",
                    component: UpdateAccountComponent
                },
                {
                    path: "/readAccount",
                    name: "readCompanyAccount",
                    component: ReadAccountComponent
                }
              ]
        },
        {
            path: "/profile/candidate",
            name: "candidateProfile",
            component: CandidateProfileComponent,
            children: [
                {
                    path: "/account/:id",
                    name: "updateCandidateAccount",
                    component: UpdateAccountComponent
                },
                {
                    path: "/readAccount",
                    name: "readCandidateAccount",
                    component: ReadAccountComponent
                }
              ]
        },
        {
            path: "/profile/collaborator",
            name: "collaboratorProfile",
            component: CollaboratorProfileComponent,
            children: [
                {
                    path: "/account/:id",
                    name: "updateCollaboratorAccount",
                    component: CollaboratorAccountComponent
                },
                {
                    path: "/account",
                    name: "readCollaboratorAccount",
                    component: CollaboratorAccountComponent
                },
                {
                    path: "/collaborator/catalog",
                    name: "catalogCollaborator",
                    component: CatalogComponent
                }
              ]
        },
        {
            path: "/account",
            name: "createAccount",
            component: CreateAccountComponent
        }
    ],
    mode: 'history'

})

var adress = Vue.component('adress', {
    name: 'adress',
    constructor: require('./account/view/Adress.vue').defaults
});
var contact = Vue.component('contact', {
    name: 'contact',
    constructor: require('./account/view/Contact.vue').defaults
});
var responsible = Vue.component('responsible', {
    name: 'responsible',
    constructor: require('./account/view/Responsible.vue').defaults
});
// component which contains the part about login/password
var auth = Vue.component('auth', {
    name: 'auth',
    constructor: require('./account/view/Auth.vue').defaults
});
var agencyAccount = Vue.component('agencyAccount', {
    name: 'agencyAccount',
    constructor: require('./account/view/agency/AgencyAccount.vue').defaults
});
var companyAccount = Vue.component('companyAccount', {
    name: 'companyAccount',
    constructor: require('./account/view/company/CompanyAccount.vue').defaults
});
var candidateAccount = Vue.component('candidateAccount', {
    name: 'candidateAccount',
    constructor: require('./account/view/candidate/CandidateAccount.vue').defaults
});
// var collaboratorAccount = Vue.component('collaboratorAccount', {
//     name: 'collaboratorAccount',
//     constructor: require('./account/view/collaborator/CollaboratorAccount.vue').defaults
// });
var fund = Vue.component('fund', {
    name: 'fund',
    constructor: require('./account/view/company/Fund.vue').defaults
});

  new Vue({
    el: '#app',
    template: "<div><router-view></router-view></div>",
    http: {},
    router,
    components: {
        'adress': adress,
        'contact': contact,
        'responsible': responsible,
        'agencyAccount': agencyAccount,
        'candidateAccount': candidateAccount,
        'companyAccount': companyAccount,
        'fund': fund
    }
  })
