# Internal Services Angular 2 Seed

## Setup

1. Install Node.js - Please check our guidelines to make sure you install the correct version
> [Web Guidelines V2](https://digit.mgsops.net/opsdev/WebGuidelinesV2)
- Install Python27 (make sure path variable exists)
- npm install -g gulp
- npm install -g typescript
- install VCRedist (VC c++ redistributable)

## RUNNING

Note that this seed project requires node v4.x.x or higher and npm 2.14.7 but in order to be able to take advantage of the complete functionality we strongly recommend [node >=v6.5.0](https://nodejs.org/en/download/) and npm >=3.10.3.

2. Run `yarn install` or `npm install` from the project root to install all packages.
```
yarn install
```
OR
```
npm install
```


3. Run `npm start` from the project root to build and load live-reload
```
npm start
```

4. To start development with live-reload site and coverage as well as continuous testing
```
$ npm run start.deving

# dev build
$ npm run build.dev

# prod build
$ npm run build.prod
```
***

## About this project

This project is a seed project for any new projects within the Internal Services space.

This project includes:

1. Token auth with Ops Auth.
2. Http interceptor that appends token to all requests.  
3. Basic 401 and 404 pages
4. Basic loading icon
5. Demo code

## Contributors
```
Wesley Coetzee
Andrew Fairlie
```