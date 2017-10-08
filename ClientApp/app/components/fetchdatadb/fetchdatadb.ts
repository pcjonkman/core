import { HttpClient, json } from 'aurelia-fetch-client';
import { inject } from 'aurelia-framework';

@inject(HttpClient)
export class FetchdataDb {
    private _http: HttpClient;
    public blog: Blog;
    public blogPost: Post;
    public blogs: Blog[];

    constructor(http: HttpClient) {
        this._http = http;

        this._http.fetch('/api/SampleDb')
            .then(result => result.json() as Promise<Blog[]>)
            .then(data => {
                this.blogs = data;
            });
    }

    public selectPost(blog: Blog) {
        this.blog = blog;
        this.blogPost = null;
    }

    public selectDelete(post: Post, blog: Blog) {
        this.blog = blog;
        this.blogPost = post;
    }

    public post() {
        if (this.blog) {
            let post = { Content: this.blogPost ? this.blogPost.content : 'Awesome!', User: this.blog.user };
            this._http.fetch('/api/SampleDb', {
                method: 'post',
                body: json(post)
            })
            .then(result => result.json() as Promise<Blog[]>)
            .then(data => {
                this.blogs = data;
                this.blog = null;
                this.blogPost = null;
            });
        }
    }

    public delete() {
        let postDelete = { Id: this.blogPost.id, User: this.blog.user }
        this._http.fetch('/api/SampleDb', {
            method: 'delete',
            body: json(postDelete)
        })
        .then(result => result.json() as Promise<Blog[]>)
        .then(data => {
            this.blogs = data;
            this.blog = null;
            this.blogPost = null;
        });
    }
}

interface Blog {
    firstName: string;
    lastName: string;
    isLoggedIn: boolean;
    lastLoginDate: string;
    posts: Post;
    user: any;
    roles: string[];
}

interface Post {
    id: string;
    content: string;
}
