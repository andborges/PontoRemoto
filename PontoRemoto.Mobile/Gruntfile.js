module.exports = function(grunt) {
	grunt.option("app", "PontoRemoto");

    grunt.initConfig({
        pkg: grunt.file.readJSON('package.json'),
		clean: {
			rebuild: ["platforms", "plugins"],
			release: ["platforms/android/bin"],
			postrelease: ["release"]
		},
		mkdir: {
			all: {
				options: {
					create: ["platforms", "plugins"]
				}
			}
		},
        exec: {
        	prepare: {
        		command: "phonegap prepare",
        		stdout: true,
        		stderror: true
        	},
			addPlugins: {
        		command: "phonegap plugin add https://git-wip-us.apache.org/repos/asf/cordova-plugin-dialogs.git && " +
				         "phonegap plugin add https://git-wip-us.apache.org/repos/asf/cordova-plugin-device.git && " +
						 "phonegap plugin add https://git-wip-us.apache.org/repos/asf/cordova-plugin-geolocation.git && " +
						 "phonegap plugin add org.apache.cordova.network-information && " +
						 "phonegap plugin add https://github.com/EddyVerbruggen/Toast-PhoneGap-Plugin.git",
        		stdout: true,
        		stderror: true
        	},
			build: {
				command: "phonegap build android",
				stdout: true,
        		stderror: true
			},
			updateProject: {
				command: "android update project -p platforms/android && android update project -p platforms/android/CordovaLib"
			},
			ant: {
				command: "cd platforms/android && ant release && cd../..",
				stdout: true,
        		stderror: true
			},
			sign: {
				command: "cd platforms/android/bin && " +
				         "jarsigner -verbose -sigalg SHA1withRSA -digestalg SHA1 -keystore ../../../build/mobileit-release-key.keystore -storepass prabor CordovaApp-release-unsigned.apk mobileit && " +
						 "zipalign -v 4 CordovaApp-release-unsigned.apk " + grunt.option("app") + ".apk && " +
						 "cd../../.."
			}
        },
		copy: {
			ant: {
				src: "build/ant.properties",
				dest: "platforms/android/ant.properties"
			},
			postrelease: {
				src: "platforms/android/bin/" + grunt.option("app") + ".apk",
				dest: "release/" + grunt.option("app") + ".apk"
			}
		},
        watch:{
        	files:['www/**/*.*'],
        	tasks:['exec:prepare']
        }
    });

	grunt.loadNpmTasks('grunt-contrib-clean');
	grunt.loadNpmTasks('grunt-contrib-copy');
	grunt.loadNpmTasks('grunt-contrib-watch');
	grunt.loadNpmTasks('grunt-exec');
	grunt.loadNpmTasks('grunt-mkdir');

    grunt.registerTask('default', ['watch']);
	grunt.registerTask('rebuild', ['clean:rebuild', 'mkdir:all', 'exec:addPlugins', 'exec:build', 'exec:updateProject']);
	grunt.registerTask('release', ['exec:build', 'copy:ant', 'clean:release', 'exec:ant', 'exec:sign', 'clean:postrelease', 'copy:postrelease']);
};