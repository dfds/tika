IMAGE_NAME = "ded/tika"
BUILD_NUMBER = "N/A"
CONFLUENT_CLI_VERSION = "v2.12.0"
CONFLUENT_CLI_ARCH = "amd64"
CONFLUENT_CLI_OS = "alpine"

build:
	docker build -t $(IMAGE_NAME) .
	
run:
	docker run -it -p 3000:3000 --rm --env-file ../.env $(IMAGE_NAME)

release: build
	chmod +x ../scripts/push_container_image.sh && ../scripts/push_container_image.sh $(IMAGE_NAME) $(BUILD_NUMBER)

prereqs: get-confluent-cli

# Requires you to authenticate before you can access the specified Bucket. Had to store the binaries separately since Confluent doesn't do this versioning thingy. 
get-confluent-cli: 
	aws s3 cp s3://confluent-cli-bins-9gfayuge7p/confluent-${CONFLUENT_CLI_VERSION}-${CONFLUENT_CLI_OS}-${CONFLUENT_CLI_ARCH} confluent-bin