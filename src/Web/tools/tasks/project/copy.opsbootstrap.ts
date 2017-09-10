import * as gulp from 'gulp';
import { join } from 'path';
import * as runSequence from 'run-sequence';

import Config from '../../config';

gulp.task('copy.opsbootstrap.css', () => {
  return  gulp.src([
       join( Config.OPS_BOOTSRAP, '/css/opsdev-bootstrap.css')
   ])
   .pipe(gulp.dest(join (Config.APP_DEST, 'css')));
});


gulp.task('copy.opsbootstrap.fonts', () => {
    return gulp.src([
        join( Config.OPS_BOOTSRAP, '/fonts/*.*'),
        join( Config.OPS_BOOTSRAP, '/fonts/bootstrap/*.*')
    ])
    .pipe(gulp.dest(join (Config.APP_DEST, 'fonts')));
});

gulp.task('copy.opsbootstrap.fonts.bootstrap', () => {
    return gulp.src([
        join( Config.OPS_BOOTSRAP, '/fonts/bootstrap/*.*')
    ])
    .pipe(gulp.dest(join (Config.APP_DEST, '/fonts/bootstrap/')));
});


export = (done: any) => {
    runSequence(
        'copy.opsbootstrap.css',
        'copy.opsbootstrap.fonts',
        'copy.opsbootstrap.fonts.bootstrap',
        done
    );
};
