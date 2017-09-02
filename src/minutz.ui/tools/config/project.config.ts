import { join } from 'path';
import { SeedConfig } from './seed.config';
import { Environments, InjectableDependency } from './seed.config.interfaces';
import Config from './../config';

/**
 * This class extends the basic seed configuration, allowing for project specific overrides. A few examples can be found
 * below.
 */

export const ENVIRONMENTS: Environments = {
  DEVELOPMENT: 'dev',
  PRODUCTION: 'prod'
};



export class ProjectConfig extends SeedConfig {

  PROJECT_TASKS_DIR = join(process.cwd(), this.TOOLS_DIR, 'tasks', 'project');



  constructor() {
    super();
    // this.APP_TITLE = 'Put name of your app here';
    /* Enable typeless compiler runs (faster) between typed compiler runs. */
    // this.TYPED_COMPILE_INTERVAL = 5;

    // Add `NPM` third-party libraries to be injected/bundled.
    this.NPM_DEPENDENCIES = [
      ...this.NPM_DEPENDENCIES,
      {src: 'jquery/dist/jquery.min.js', inject: 'libs'},
      {src: 'jqueryui/jquery-ui.js', inject: 'libs'},
      {src: 'bootstrap/dist/js/bootstrap.js', inject: 'libs'},
      {src: 'raphael/raphael.js', inject: 'libs'},
      {src: 'morris.js/morris.js', inject: 'libs'},
      {src: 'jquery-sparkline/jquery.sparkline.js', inject: 'libs'},
      {src: 'jquery-knob/dist/jquery.knob.min.js', inject: 'libs'},
      {src: 'moment/moment.js', inject: 'libs'},
      {src: 'daterangepicker/daterangepicker.js', inject: 'libs'},
      {src: 'bootstrap-datepicker/js/bootstrap-datepicker.js', inject: 'libs'},
      {src: 'admin-lte/plugins/timepicker/bootstrap-timepicker.js', inject: 'libs'},
      {src: 'jquery-slimscroll/jquery.slimscroll.js', inject: 'libs'},
      {src: 'bootstrap3-wysihtml5-bower/dist/bootstrap3-wysihtml5.all.js', inject: 'libs'},
      {src: 'admin-lte/dist/js/app.js', inject: 'libs'},
      {src: 'admin-lte/plugins/iCheck/icheck.js', inject: 'libs'},
      {src: 'admin-lte/plugins/fullcalendar/fullcalendar.js', inject: 'libs'},
      {src: 'admin-lte/plugins/fullcalendar/fullcalendar.css', inject: true},
      {src: 'admin-lte/plugins/fullcalendar/fullcalendar.print.css', inject: true},
      {src: 'select2/dist/js/select2.full.min.js', inject: 'libs'},
      {src: 'auth0-js/build/auth0.js', inject: 'libs'},
      {src: 'select2/dist/css/select2.min.css', inject: true},
      {src: 'dropzone/dist/dropzone.js', inject: 'libs'},
      
      {src: 'bootstrap/dist/css/bootstrap.css', inject: true},
      {src: 'font-awesome/css/font-awesome.css', inject: true},
      // {src: 'ionicons/css/ionicons.css', inject: true},
      // {src: 'icheck/skins/flat/blue.css', inject: true},
      {src: 'morris.js/morris.css', inject: true},
      {src: 'better-timezone/dist/better-timezone', inject: 'libs'},
      {src: 'bootstrap-datepicker/dist/css/bootstrap-datepicker3.css', inject: true},
      {src: 'admin-lte/plugins/daterangepicker/daterangepicker.css', inject: true},
      {src: 'bootstrap3-wysihtml5-bower/dist/bootstrap3-wysihtml5.css', inject: true},
      {src: 'admin-lte/dist/css/AdminLTE.css', inject: true},
      {src: 'admin-lte/dist/css/skins/skin-blue.css', inject: true},
      {src: 'admin-lte/dist/css/alt/AdminLTE-fullcalendar.css', inject: true},
      {src: 'admin-lte/dist/css/alt/AdminLTE-select2.css', inject: true},
      {src: 'admin-lte/dist/css/alt/AdminLTE-bootstrap-social.css', inject: true},
      {src: 'admin-lte/plugins/timepicker/bootstrap-timepicker.css', inject: true},
      {src: 'admin-lte/plugins/iCheck/all.css', inject: true},
      {src: 'dropzone/dist/basic.css', inject: true},
      {src: 'dropzone/dist/dropzone.css', inject: true}
    ];
    // let additionalPackages: ExtendPackages[] = [
    //   {
    //     name:'jquery',
    //     path:'node_modules/jquery/dist/jquery.min.js'
    //   },
    //   {
    //     name:'select2',
    //     path:'node_modules/select2/dist/js/select2.full.min.js'
    //   },
    //   {
    //     name:'auth0-js',
    //     path:'node_modules/auth0-js/build/auth0.js'
    //   },
    //   {
    //     name:'jquery-ui',
    //     path:'node_modules/jqueryui/jquery-ui.js'
    //   },
    //   {
    //     name:'bootstrap',
    //     path:'node_modules/bootstrap/dist/js/bootstrap.js'
    //   },   
    //   {
    //     name:'raphael',
    //     path:'node_modules/raphael/raphael.js'
    //   },
    //   {
    //     name:'morris',
    //     path:'node_modules/morris.js/morris.js'
    //   },
    //   {
    //     name:'jquery-sparkeline',
    //     path:'node_modules/jquery-sparkline/jquery-sparkline'
    //   },
    //   {
    //     name:'jquery-knob',
    //     path:'node_modules/jquery-knob/dist/jquery.knob.min.js'
    //   },
    //   {
    //     name:'moment',
    //     path:'node_modules/moment/moment.js'
    //   },
    //   {
    //     name:'daterangepicker',
    //     path:'node_modules/daterangepicker/daterangepicker.js'
    //   },
    //   {
    //     name:'bootstrap-datepicker',
    //     path:'node_modules/bootstrap-datepicker/js/bootstrap-datepicker.js'
    //   },
    //   {
    //     name:'jquery-slimscroll',
    //     path:'node_modules/jquery-slimscroll/jquery.slimscroll.js'
    //   },
    //   {
    //     name:'bootstrap3-wysihtml5-bower',
    //     path:'node_modules/bootstrap3-wysihtml5-bower/dist/bootstrap3-wysihtml5.all.js'
    //   },
    //   {
    //     name:'icheck',
    //     path:'node_modules/icheck/icheck.js'
    //   },
    //   {
    //     name:'admin-lte',
    //     path:'node_modules/admin-lte/dist/js/app.js'
    //   },
      
      // mandatory dependency for ngx-bootstrap datepicker 
      // {
      //   name:'moment',
      //   path:'node_modules/moment',
      //   packageMeta:{
      //     main: 'moment.js',
      //     defaultExtension: 'js'
      //   }
      // }
      //];    
      //this.addPackagesBundles(additionalPackages);

    // Add `local` third-party libraries to be injected/bundled.
    this.APP_ASSETS = [
      ...this.APP_ASSETS,
      //  {src: `${this.APP_SRC}/client/css/AdminLTE.css`, inject: true, vendor: false},//adminlte
      //  {src: `${this.APP_SRC}/client/js/datepicker/datepicker3.css`, inject: true, vendor: false},//datepicker3
      //  {src: `${this.APP_SRC}/client/js/daterangepicker/daterangepicker.css`, inject: true, vendor: false},//daterangepicker
      //  {src: `${this.APP_SRC}/client/js/bootstrap-wysihtml5/bootstrap3-wysihtml5.css`, inject: true, vendor: false},
       {src: `${this.APP_SRC}/css/site.css`, inject: true, vendor: false},
       {src: `${this.APP_SRC}/js/scripts.js`,inject: true, vendor: false},
      //  {src: `${this.APP_SRC}/client/js/adminlte/adminlte.js`, inject: 'libs'},
      //  {src: `${this.APP_SRC}/client/js/datepicker/bootstrap-datepicker.js`, inject: 'libs'},
      //  {src: `${this.APP_SRC}/client/js/momentjs/moment.min.js`, inject: 'libs'},
      //  {src: `${this.APP_SRC}/client/js/momentjs/moment-timezone-with-data-2012-2022.min.js`, inject: 'libs'},
      //  {src: `${this.APP_SRC}/client/js/bootstrap-wysihtml5/bootstrap3-wysihtml5.all.js`, inject: 'libs'},
      //  {src: `${this.APP_SRC}/client/js/daterangepicker/daterangepicker.js`, inject: 'libs'}
       {src: `${this.APP_SRC}/js/bootstrap-toggle.js`, inject: true, vendor: false},
       {src: `${this.APP_SRC}/js/jquery-time-duration-picker.min.js`, inject: true, vendor: false},
       {src: `${this.APP_SRC}/css/bootstrap-toggle.css`, inject: true, vendor: false}
    ];

    /* Add to or override NPM module configurations: */
    // this.mergeObject(this.PLUGIN_CONFIGS['browser-sync'], { ghostMode: false });
    // Add packages (e.g. angular2-jwt)

  }

}
