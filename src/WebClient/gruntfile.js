module.exports = function(grunt) {

  grunt.initConfig({
    less: {
      development: {
        options: {
          sourceMap: true,
          sourceMapFilename: 'Content/bootstrap/bootstrap.css.map',
          sourceMapURL: "Content/bootstrap/bootstrap.css.map"
        },
        files: {
          // target.css file: source.less file
          "Content/bootstrap/bootstrap.css": "Content/bootstrap/bootstrap.less"
        }
      }
    },
    watch: {
      styles: {
        // Which files to watch (all .less files recursively in the less directory)
        files: ['Content/bootstrap/*.less'],
        tasks: ['less'],
        options: {
          livereload: true
        }
      },
      sprite: {
        tasks: ['sprite'],
        files: ['Content/img/sprite_src/*']
      }
    },
    sprite: {
        portal: {
            src: 'Content/img/sprite_src/*.png',
            destImg: 'Content/img/sprite.png',
            destCSS: 'Content/bootstrap/sprite.less',
            imgPath: '../img/sprite.png',
            engine: 'pngsmith'
        }
    }
  });
 
  grunt.loadNpmTasks('grunt-spritesmith');
  grunt.loadNpmTasks('grunt-contrib-less');
  grunt.loadNpmTasks('grunt-contrib-watch');
 
  grunt.registerTask('default', ['less','sprite']);

};