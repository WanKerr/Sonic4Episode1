
import type { UnpackedFiles, UnpackedDirectory, FileMetadata, DirectoryMetadata, FileData } from './types.js'

import { Encoder } from '@msgpack/msgpack'
import { Buffer } from 'buffer'

import { createEmpty } from './pickle.js'
import { isDirectory, isDirectoryMetadata } from './utils.js'

import path from 'path-browserify'

export * from './types.js'

const encoder = new Encoder();
const makeFlatTree = (files: UnpackedFiles): UnpackedFiles => {
  const tree: UnpackedFiles = {}

  for (const [key, val] of Object.entries(files)) {
    let currDir = tree
    const dirs = key.split(path.sep).filter(Boolean)
    const filename = <string>dirs.pop()
    for (const dir of dirs) {
      currDir = <UnpackedFiles>(currDir[dir] = currDir[dir] ?? {})
    }
    currDir[filename] = val
  }

  return <UnpackedDirectory>tree
}

const makeHeaderTree = (files: UnpackedFiles): UnpackedDirectory => {
  const tree: UnpackedDirectory = { files: {} }
  for (const [key, value] of Object.entries(files)) {
    if (isDirectory(value)) {
      tree.files[key] = makeHeaderTree(<UnpackedFiles>value)
    } else {
      tree.files[key] = <FileData>value
    }
  }

  return tree
}

const makeSizeTree = (tree: UnpackedDirectory): DirectoryMetadata => {
  let files: DirectoryMetadata = { files: {} }
  for (const [key, value] of Object.entries(tree.files)) {
    if (isDirectoryMetadata(value as any)) {
      files.files[key] = makeSizeTree(<UnpackedDirectory>value)
    } else {
      // files.files[key] = { size: (<any>value)?.length }
      files.files[key] = [0, (<any>value)?.length]
    }
  }

  return files
}

const makeOffsetTree = (tree: DirectoryMetadata): DirectoryMetadata => {
  const makeInnerOffsetTree = (tree: DirectoryMetadata, offset: number): [DirectoryMetadata, number] => {
    for (const [key, value] of Object.entries(tree.files)) {
      if (isDirectoryMetadata(value)) {
        const [newValue, newOffset] = makeInnerOffsetTree(<DirectoryMetadata>value, offset)
        tree.files[key] = newValue
        offset = newOffset
      } else {
        // tree.files[key] = { size: (<FileMetadata>value).size || 0, offset: offset }
        tree.files[key] = [offset, (<FileMetadata>value)[1] || 0]
        offset += (<FileMetadata>value)[1] || 0
      }
    }

    return [tree, offset]
  }

  return makeInnerOffsetTree(tree, 0)[0]
}

const makeHeader = (files: UnpackedFiles): DirectoryMetadata =>
  makeOffsetTree(
    makeSizeTree(
      makeHeaderTree(files)
    )
  )

const makeFilesBuffer = (files: UnpackedFiles): Buffer[] =>
  Object.entries(files)
    .reduce<Buffer[]>((arr, [, value]) => [
      ...arr,
      ...(
        isDirectory(value) ? makeFilesBuffer(<UnpackedFiles>value)
          : [Buffer.from(value as any)]
      )
    ], [])

export const createPackage = async (files: UnpackedFiles, { flat = false } = {}): Promise<Buffer> => {
  const header = makeHeader(flat ? makeFlatTree(files) : files)
  // const headerPickle = createEmpty()
  // headerPickle.writeString(JSON.stringify(header))
  // const headerBuf = headerPickle.toBuffer()

  const headerPickle = createEmpty();
  const headerData = encoder.encode(header);
  headerPickle.writeInt(headerData.length);
  headerPickle.writeBytes(headerData, headerData.length);
  const headerBuf = headerPickle.toBuffer();

  const sizePickle = createEmpty()
  sizePickle.writeUInt32(headerBuf.length)
  const sizeBuf = sizePickle.toBuffer()
  return Buffer.concat([sizeBuf, headerBuf, ...makeFilesBuffer(files)])
}
