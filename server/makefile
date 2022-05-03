IMAGE_NAME = "ded/tika"
BUILD_NUMBER = "N/A"

build:
	docker build -t $(IMAGE_NAME) .
	
run:
	docker run -it -p 3000:3000 --rm --env-file ../.env $(IMAGE_NAME)

release: build
	chmod +x ../scripts/push_container_image.sh && ../scripts/push_container_image.sh $(IMAGE_NAME) $(BUILD_NUMBER)
	chmod +x ../scripts/update_mainfests.sh && ../scripts/update_mainfests.sh $(IMAGE_NAME) $(BUILD_NUMBER)