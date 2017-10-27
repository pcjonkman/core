import 'moment/locale/nl';
import { Aurelia, PLATFORM } from 'aurelia-framework';
import { HttpClient, HttpClientConfiguration } from 'aurelia-fetch-client';
import { Router, RouterConfiguration } from 'aurelia-router';
import { autoinject } from 'aurelia-framework';
import * as moment from 'moment';
import * as toastr from 'toastr';

@autoinject()
export class App {
    private _httpClient: HttpClient;
    private router: Router;

    public constructor(httpClient: HttpClient) {
        this._httpClient = httpClient;

        this._httpClient.configure((config: HttpClientConfiguration) => {
            config
              .withDefaults({
                credentials: 'same-origin',
                headers: {
                  Accept: 'application/json'
                }
              });
          });

        // Set moment to Dutch locale.
        moment.locale('nl');
        toastr.options = {
            "closeButton": false,
            "debug": false,
            "newestOnTop": true,
            "progressBar": false,
            "positionClass": "toast-bottom-center",
            "preventDuplicates": true,
            "onclick": null,
            "showDuration": "300",
            "hideDuration": "1000",
            "timeOut": "3000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "slideDown",
            "hideMethod": "slideUp"
          }
    }

    configureRouter(config: RouterConfiguration, router: Router) {
        config.title = 'Aurelia';
        config.map([{
            route: 'home',
            name: 'home',
            settings: { icon: 'home' },
            moduleId: PLATFORM.moduleName('../home/home'),
            nav: true,
            title: 'Home'
        }, {
            route: 'counter',
            name: 'counter',
            settings: { icon: 'education' },
            moduleId: PLATFORM.moduleName('../counter/counter'),
            nav: true,
            title: 'Counter'
        }, {
            route: 'fetch-data',
            name: 'fetchdata',
            settings: { icon: 'th-list' },
            moduleId: PLATFORM.moduleName('../fetchdata/fetchdata'),
            nav: true,
            title: 'Fetch data'
        }, {
            route: 'fetch-data-db',
            name: 'fetchdatadb',
            settings: { icon: 'th' },
            moduleId: PLATFORM.moduleName('../fetchdatadb/fetchdatadb'),
            nav: true,
            title: 'Fetch data db'
        }, {
            route: [ '', 'pool' ],
            name: 'pool',
            settings: { icon: 'globe' },
            moduleId: PLATFORM.moduleName('../pool/pool'),
            nav: true,
            title: 'Pool'
        }, {
            route: 'pool-players',
            name: 'poolplayers',
            settings: { icon: 'tasks' },
            moduleId: PLATFORM.moduleName('../pool/poolplayers'),
            nav: true,
            title: 'PoolPlayers'
        }, {
            route: 'schedule',
            name: 'schedule',
            settings: { icon: 'tasks' },
            moduleId: PLATFORM.moduleName('../pool/schedule'),
            nav: true,
            title: 'Schedule'
        }, {
            route: 'core',
            name: 'core',
            settings: { icon: 'cog' },
            moduleId: PLATFORM.moduleName('../home/core'),
            nav: true,
            title: 'Users'
        }]);

        this.router = router;
    }
}
