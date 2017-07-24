node('maven') {

    stage('checkout') {
       echo "checking out source"
       echo "Build: ${BUILD_ID}"
       checkout scm
    }

    stage('code quality check') {
            SONARQUBE_PWD = sh (
             script: 'oc env dc/sonarqube --list | awk  -F  "=" \'/SONARQUBE_ADMINPW/{print $2}\'',
             returnStdout: true
              ).trim()
           echo "SONARQUBE_PWD: ${SONARQUBE_PWD}"

           SONARQUBE_URL = sh (
               script: 'oc get routes -o wide --no-headers | awk \'/sonarqube/{ print match($0,/edge/) ?  "https://"$2 : "http://"$2 }\'',
               returnStdout: true
                  ).trim()
           echo "SONARQUBE_URL: ${SONARQUBE_URL}"

           dir('sonar-runner') {
            sh returnStdout: true, script: "./gradlew sonarqube -Dsonar.host.url=${SONARQUBE_URL} -Dsonar.verbose=true --stacktrace --info  -Dsonar.sources=.."
        }
    }
	stage('build') {
	 echo "Building..."
	 openshiftBuild bldCfg: 'nmp', showBuildLogs: 'true'
	 openshiftTag destStream: 'nmp', verbose: 'true', destTag: '$BUILD_ID', srcStream: 'nmp', srcTag: 'latest'
	 openshiftTag destStream: 'nmp', verbose: 'true', destTag: 'dev', srcStream: 'nmp', srcTag: 'latest'
    }
	
	stage('validation') {
          dir('functional-tests'){
                 sh './gradlew --debug --stacktrace phantomJsTest'
      }
   }
}


stage('deploy-test') {
  input "Deploy to test?"
  
  node('master'){
     openshiftTag destStream: 'nmp', verbose: 'true', destTag: 'test', srcStream: 'nmp', srcTag: '$BUILD_ID'
  }
}

stage('deploy-prod') {
  input "Deploy to prod?"
  node('master'){
     openshiftTag destStream: 'nmp', verbose: 'true', destTag: 'prod', srcStream: 'nmp', srcTag: '$BUILD_ID'
  }
  
}

