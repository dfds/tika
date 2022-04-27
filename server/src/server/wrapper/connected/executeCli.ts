import { spawn } from "child_process";
import * as readline from "readline";
import { CcloudSessionExpiredException, CliException } from "./../model/error";

export function executeCli(args: string[]): Promise<string[]> {
  const cli = process.env.TIKA_CCLOUD_BIN_PATH ?? "confluent";

  return new Promise((resolve, reject) => {
    const lines: Array<string> = [];
    const errLines: Array<string> = [];
    const runner = spawn(cli, args);
    const reader = readline.createInterface({ input: runner.stdout });
    const errReader = readline.createInterface({ input: runner.stderr });

    reader.on("line", data => lines.push((data as any).toString("utf8")));
    errReader.on("line", data => errLines.push((data as any).toString("utf8")));

    runner.on("exit", exitCode => {

      if (exitCode.toString() == "0") {
        return resolve(lines);
      }

      if (errLines.some((l: string): boolean => l.includes("You must be logged in to run this command"))) {
        return reject(new CcloudSessionExpiredException());
      }

      reject(new CliException(exitCode, errLines));
    });
  });
}