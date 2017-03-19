import { HttpClient } from 'aurelia-fetch-client';
import { inject } from 'aurelia-framework';

@inject(HttpClient)
export class FetchdataDb {
    public blogs: Blog[];

    constructor(http: HttpClient) {
        http.fetch('/api/SampleDb')
            .then(result => result.json() as Promise<Blog[]>)
            .then(data => {
                this.blogs = data;
            });
    }
}

interface Blog {
    firstName: string;
    lastName: string;
    posts: string[];
}
