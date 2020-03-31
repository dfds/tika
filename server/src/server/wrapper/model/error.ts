export class CcloudSessionExpiredException extends Error {
  constructor(args: any = null) {
    super(args);
    this.name = "CcloudSessionExpired";
    this.message = "Login session has expired";
  }
}

export class TopicAlreadyExistsException extends Error {
  constructor(args: any = null) {
    super(args);
    this.name = "CcloudTopicAlreadyExists";
    this.message = "A Topic with the given name already exists";
  }
}

export class ServiceAccountAlreadyExistsException extends Error {
  constructor(args: any = null) {
    super(args);
    this.name = "CcloudServiceAccountAlreadyExists";
    this.message = "A Service account with the given name already exists";
  }
}

export class CliException extends Error {
  exitCode: number;
  consoleLines: string[];

  constructor(exitCode: number, consoleLines: string[]) {
    super(null);
    this.exitCode = exitCode;
    this.consoleLines = consoleLines;
    this.name = "CliException";
    this.message = "A Service account with the given name already exists";
  }
}