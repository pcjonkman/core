import 'moment/locale/nl';
import { Aurelia, PLATFORM } from 'aurelia-framework';
import { HttpClient, HttpClientConfiguration } from 'aurelia-fetch-client';
import { Router, RouterConfiguration } from 'aurelia-router';
import { autoinject } from 'aurelia-framework';
import * as moment from 'moment';
import * as toastr from 'toastr';

@autoinject()
export class App {
    private router: Router;

    public constructor(private readonly _httpClient: HttpClient) {

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
        toastr.options.closeButton = false;
        toastr.options.debug = false;
        toastr.options.newestOnTop = true;
        toastr.options.progressBar = false;
        toastr.options.positionClass = "toast-bottom-center";
        toastr.options.preventDuplicates = true;
        toastr.options.onclick = null;
        toastr.options.showDuration = 300;
        toastr.options.hideDuration = 1000;
        toastr.options.timeOut = 3000;
        toastr.options.extendedTimeOut = 1000;
        toastr.options.showEasing = "swing";
        toastr.options.hideEasing = "linear";
        toastr.options.showMethod = "slideDown";
        toastr.options.hideMethod = "slideUp";
    }

    configureRouter(config: RouterConfiguration, router: Router) {
        config.title = 'Aurelia';
        config.map([
        {
        //     route: 'home',
        //     name: 'home',
        //     settings: { icon: 'home' },
        //     moduleId: PLATFORM.moduleName('../home/home'),
        //     nav: true,
        //     title: 'Home'
        // }, {
        //     route: 'counter',
        //     name: 'counter',
        //     settings: { icon: 'education' },
        //     moduleId: PLATFORM.moduleName('../counter/counter'),
        //     nav: true,
        //     title: 'Counter'
        // }, {
        //     route: 'fetch-data',
        //     name: 'fetchdata',
        //     settings: { icon: 'th-list' },
        //     moduleId: PLATFORM.moduleName('../fetchdata/fetchdata'),
        //     nav: true,
        //     title: 'Fetch data'
        // }, {
        //     route: 'fetch-data-db',
        //     name: 'fetchdatadb',
        //     settings: { icon: 'th' },
        //     moduleId: PLATFORM.moduleName('../fetchdatadb/fetchdatadb'),
        //     nav: true,
        //     title: 'Fetch data db'
        // }, {
            route: [ '', 'pool' ],
            name: 'pool',
            settings: { icon: 'globe' },
            moduleId: PLATFORM.moduleName('../pool/pool'),
            nav: true,
            title: 'Pool'
        }, {
            route: 'pool-players',
            name: 'poolplayers',
            settings: { icon: 'user' },
            moduleId: PLATFORM.moduleName('../pool/poolplayers'),
            nav: true,
            title: 'PoolPlayers'
        }, {
            route: 'schedule',
            name: 'schedule',
            settings: { icon: 'stats' },
            moduleId: PLATFORM.moduleName('../pool/schedule'),
            nav: true,
            title: 'Schedule'
        }, {
            route: ['predictions', 'predictions/:id'],
            name: 'predictions',
            settings: { icon: 'list-alt' },
            moduleId: PLATFORM.moduleName('../pool/predictions'),
            nav: true,
            title: 'Predictions'
        }, {
            route: 'results',
            name: 'results',
            settings: { icon: 'flag' },
            moduleId: PLATFORM.moduleName('../pool/results'),
            nav: true,
            title: 'Results'
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
