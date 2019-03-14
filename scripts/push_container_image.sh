IMAGE_NAME=${1}
BUILD_NUMBER=${2}

aws ecr get-login --no-include-email
registry_image_name="579478677147.dkr.ecr.eu-central-1.amazonaws.com/${IMAGE_NAME}:${BUILD_NUMBER}"

docker tag ${IMAGE_NAME}:latest ${registry_image_name}
docker push ${registry_image_name}