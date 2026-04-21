#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "$0")" && pwd)"
PROJECT_DIR="$ROOT_DIR/src/AntlrPredictionMode"
GRAMMAR_FILE="$PROJECT_DIR/Grammar/PredictionDemo.g4"
OUTPUT_DIR="$PROJECT_DIR/Generated"

rm -rf "$OUTPUT_DIR"
mkdir -p "$OUTPUT_DIR"

/usr/bin/antlr4 -Dlanguage=CSharp -visitor -no-listener -o "$OUTPUT_DIR" "$GRAMMAR_FILE"

echo "Generated parser files in: $OUTPUT_DIR"
