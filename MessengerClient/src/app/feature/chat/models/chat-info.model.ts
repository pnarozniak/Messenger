export class ChatInfo {
    name: string
    isPrivate: boolean

    constructor(name: string, isPrivate: boolean) {
        this.name = name;
        this.isPrivate = isPrivate;
    }

    static deserialize(input: any) {
        const name = input.name || input.members.map((m: any) => `${m.firstName} ${m.lastName}`).join(', ');
        return new ChatInfo(name, input.isPrivate as boolean);
    }
}