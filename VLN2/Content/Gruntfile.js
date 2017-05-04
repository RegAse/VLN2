module.exports = function (grunt) {
    grunt.initConfig({
        less: {
            development: {
                options: {
                    paths: ["less"],
                    yuicompress: true
                },
                files: {
                    "css/app.css": "less/app.less",
                    "css/bootstrap.css": "bootstrap-less/bootstrap.less"
                }
            }
        },
        watch: {
            files: ["less/*", "bootstrap-less/*"],
            tasks: ["less"]
        }
    });
    grunt.loadNpmTasks('grunt-contrib-less');
    grunt.loadNpmTasks('grunt-contrib-watch');
    grunt.registerTask('default','watch')
};