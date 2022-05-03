export class ListTopic {
    name: string
}

export type ListTopics = Array<ListTopic>;

export class DescribeTopic {
    topic_name: string
    config: {[key: string]: any}
}