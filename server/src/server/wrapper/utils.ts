export function Deserializer<T>(json:string) : T {
  let payload = JSON.parse(json);
  return payload;
}

export function ConcatOutput(output:Array<string>) : string {
  return output.join("\n");
}

export default Deserializer;