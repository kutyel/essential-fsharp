namespace ComputationExpression

module AsyncDemo =
    open System.IO

    type FileResult = { Name: string; Length: int }

    let getFileInformation path =
        async {
            let! bytes = File.ReadAllBytesAsync(path) |> Async.AwaitTask
            let fileName = Path.GetFileName(path)

            return
                { Name = fileName
                  Length = bytes.Length }
        }
