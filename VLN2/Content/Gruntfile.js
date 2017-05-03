module.exports = function (grunt) {
    grunt.initConfig({
        less: {
            development: {
                options: {
                    paths: ["less"],
                    yuicompress: true
                },
                files: {
                    "css/app.css": "less/app.less"
                }
            }
        },
        watch: {
            files: "less/*",
            tasks: ["less"]
        }
    });
    grunt.loadNpmTasks('grunt-contrib-less');
    grunt.loadNpmTasks('grunt-contrib-watch');
    grunt.registerTask('default','watch')
};