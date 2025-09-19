def PROJECT_NAME = "jenkins-unity-test"
def CUSTOM_WORKSPACE = "Z:\\Work\\Jenkins\\Unity_Projects\\${PROJECT_NAME}"
def UNITY_VERSION = "6000.1.11f1"
def UNITY_INSTALLATION = "Z:\\Work\\Unity\\Editor\\${UNITY_VERSION}\\Editor\\"

pipeline{
    environment{
        PROJECT_PATH = "${CUSTOM_WORKSPACE}\"${PROJECT_NAME}"
_
        TEST_PROJECT_KEYSTORE_FILE = credentials('TEST_PROJECT_KEYSTORE_FILE')
        KEYSTORE_PASS = credentials('KEYSTORE_PASS')
        ALIAS_NAME = credentials('ALIAS_NAME')
        ALIAS_PASS = credentials('ALIAS_PASS')
        BUNDLE_ID = "com.Garunnir.suika" //Your bundle ID. Ex: com.defaultcompany.test
        MAC_IP_ADDRESS = "" //Your mac IP address
        MAC_USERNAME = "" //Your mac username 
    }

    agent{
        label{
            label ""
            customWorkspace "${CUSTOM_WORKSPACE}"
        }
    }

    stages{
        stage('Build Windows'){
            when{expression {BUILD_WINDOWS == 'true'}}
            steps{
                script{
                    withEnv(["UNITY_PATH=${UNITY_INSTALLATION}"]){ 
                        bat '''
                        "%UNITY_PATH%/Unity.exe" -quit -batchmode -projectPath %PROJECT_PATH% -executeMethod BuildScript.BuildWindows -logFile -
                        '''
                    }
                }
            }
        }

        stage('Build WebGL'){
            when{expression {BUILD_WEBGL == 'true'}}
            steps{
                script{
                    withEnv(["UNITY_PATH=${UNITY_INSTALLATION}"]){ 
                        bat '''
                        "%UNITY_PATH%/Unity.exe" -quit -batchmode -projectPath %PROJECT_PATH% -executeMethod BuildScript.BuildWebGL -logFile -
                        '''
                    }
                }
            }
        }

        stage('Build Android APK'){
            when{expression {BUILD_ANDROID_APK == 'true'}}
            steps{
                script{
                    withEnv(["UNITY_PATH=${UNITY_INSTALLATION}"]){ 
                        bat '''
                        "%UNITY_PATH%/Unity.exe" -quit -batchmode -projectPath %PROJECT_PATH% -executeMethod BuildScript.BuildAndroid -buildType APK -logFile -
                        '''
                    }
                }
            }
        }

        stage('Build Android AAB'){
            when{expression {BUILD_ANDROID_AAB == 'true'}}
            steps{
                script{
                    withEnv(["UNITY_PATH=${UNITY_INSTALLATION}"]){ 
                        bat '''
                        "%UNITY_PATH%/Unity.exe" -quit -batchmode -projectPath %PROJECT_PATH% -executeMethod BuildScript.BuildAndroid -buildType AAB -logFile -
                        '''
                    }
                }
            }
        }

        stage('Deploy Android - Google Play'){
            when{expression {DEPLOY_ANDROID_AAB == 'true'}}
            environment{
                GOOGLE_PLAY_API_JSON_LOCATION = credentials('GOOGLE_PLAY_API_JSONLOCATION')
            }
            steps{
                script{
                    bat '''
                    fastlane supply ^
                    --track internal ^
                    --release_status "draft" ^
                    --aab "%PROJECT_PATH%/Builds/AndroidAAB/TestGame.aab" ^
                    --json_key "%GOOGLE_PLAY_API_JSON_LOCATION%" ^
                    --package_name "%BUNDLE_ID%" ^
                    --version_name "V%BUILD_NUMBER% - Test Project"
                    '''
                }
            }
        }

        stage('Build iOS'){
            when{expression {BUILD_IOS == 'true'}}
            steps{
                script{
                    withEnv(["UNITY_PATH=${UNITY_INSTALLATION}"]){ 
                        bat '''
                        "%UNITY_PATH%/Unity.exe" -quit -batchmode -projectPath %PROJECT_PATH% -executeMethod BuildScript.BuildIOS -logFile -
                        '''
                    }
                }
            }
        }

        stage('Deploy iOS Mac'){
            when{expression {DEPLOY_IOS_MAC == 'true'}}
            environment{
                MAC_PASSWORD = credentials('MAC_PASSWORD')
            }
            steps{
                script{
                    env.PROJECT_NAME = PROJECT_NAME

                    powershell '''
                    net use \\%MAC_IP_ADDRESS% /user:%MAC_USERNAME% $env:MAC_PASSWORD
                    Remove-Item -Path \\%MAC_IP_ADDRESS%\\%MAC_USERNAME%\\Desktop\\Jenkins_Builds\\$env:PROJECT_NAME -Recurse -Force -ErrorAction Ignore
                    New-Item -ItemType directory -Path \\%MAC_IP_ADDRESS%\\%MAC_USERNAME%\\Desktop\\Jenkins_Builds\\$env:PROJECT_NAME -Force
                    Copy-Item -Path "$env:PROJECT_PATH\\Builds\\iOS" -Destination \\%MAC_IP_ADDRESS%\\%MAC_USERNAME%\\Desktop\\Jenkins_Builds\\$env:PROJECT_NAME -Recurse -Force
                    net use \\%MAC_IP_ADDRESS% /delete
                    '''
                }
            }
        }
    }
}

