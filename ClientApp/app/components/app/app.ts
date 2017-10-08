import { Aurelia, PLATFORM } from 'aurelia-framework';
import { HttpClient, HttpClientConfiguration } from 'aurelia-fetch-client';
import { Router, RouterConfiguration } from 'aurelia-router';
import { autoinject } from 'aurelia-framework';

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
              
    }

    configureRouter(config: RouterConfiguration, router: Router) {
        config.title = 'Aurelia';
        config.map([{
            route: [ '', 'home' ],
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
            route: 'core',
            name: 'core',
            settings: { icon: 'user' },
            moduleId: PLATFORM.moduleName('../home/core'),
            nav: true,
            title: 'Users'
        }]);

        this.router = router;
    }
}
