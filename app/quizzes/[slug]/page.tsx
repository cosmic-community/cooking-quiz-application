// app/quizzes/[slug]/page.tsx
'use client'

import { useEffect, useState } from 'react'
import { useRouter } from 'next/navigation'
import { Clock, ChevronRight } from 'lucide-react'
import { cosmic } from '@/lib/cosmic'
import type { Quiz, QuizSession, AnswerRecord } from '@/types'
import QuizQuestion from '@/components/QuizQuestion'
import QuizResults from '@/components/QuizResults'
import Timer from '@/components/Timer'

interface PageProps {
  params: Promise<{ slug: string }>
}

export default function QuizPage({ params }: PageProps) {
  const router = useRouter()
  const [slug, setSlug] = useState<string>('')
  const [quiz, setQuiz] = useState<Quiz | null>(null)
  const [session, setSession] = useState<QuizSession | null>(null)
  const [loading, setLoading] = useState(true)
  const [timeUp, setTimeUp] = useState(false)

  useEffect(() => {
    async function getParams() {
      const resolvedParams = await params
      setSlug(resolvedParams.slug)
    }
    getParams()
  }, [params])

  useEffect(() => {
    if (slug) {
      loadQuiz()
    }
  }, [slug])

  async function loadQuiz() {
    try {
      const response = await cosmic.objects
        .findOne({
          type: 'quizzes',
          slug
        })
        .depth(2)
      
      const quizData = response.object as Quiz
      setQuiz(quizData)
      
      // Initialize quiz session
      if (quizData.metadata.questions && quizData.metadata.questions.length > 0) {
        setSession({
          quiz: quizData,
          currentQuestionIndex: 0,
          answers: [],
          startTime: new Date(),
          timeRemaining: (quizData.metadata.time_limit || 10) * 60,
          isCompleted: false
        })
      }
    } catch (error) {
      console.error('Error loading quiz:', error)
    } finally {
      setLoading(false)
    }
  }

  function handleAnswer(selectedOption: number) {
    if (!session || !quiz) return

    const currentQuestion = quiz.metadata.questions[session.currentQuestionIndex]
    if (!currentQuestion) return

    const isCorrect = selectedOption === currentQuestion.metadata.correct_answer

    const newAnswer: AnswerRecord = {
      question_id: currentQuestion.id,
      selected_option: selectedOption,
      is_correct: isCorrect
    }

    const updatedAnswers = [...session.answers, newAnswer]

    if (session.currentQuestionIndex < quiz.metadata.questions.length - 1) {
      setSession({
        ...session,
        currentQuestionIndex: session.currentQuestionIndex + 1,
        answers: updatedAnswers
      })
    } else {
      setSession({
        ...session,
        answers: updatedAnswers,
        isCompleted: true
      })
    }
  }

  function handleTimeUp() {
    setTimeUp(true)
    if (session) {
      setSession({
        ...session,
        isCompleted: true
      })
    }
  }

  function calculateScore() {
    if (!session) return 0
    const correctAnswers = session.answers.filter(a => a.is_correct).length
    const totalQuestions = quiz?.metadata.questions.length || 1
    return Math.round((correctAnswers / totalQuestions) * 100)
  }

  if (loading) {
    return (
      <div className="container mx-auto px-4 py-8">
        <div className="text-center">Loading quiz...</div>
      </div>
    )
  }

  if (!quiz || !quiz.metadata.questions || quiz.metadata.questions.length === 0) {
    return (
      <div className="container mx-auto px-4 py-8">
        <div className="text-center">
          <p className="text-gray-600">Quiz not found or has no questions.</p>
        </div>
      </div>
    )
  }

  if (session?.isCompleted) {
    return (
      <QuizResults
        quiz={quiz}
        answers={session.answers}
        score={calculateScore()}
        totalQuestions={quiz.metadata.questions.length}
      />
    )
  }

  const currentQuestion = session ? quiz.metadata.questions[session.currentQuestionIndex] : null

  return (
    <div className="container mx-auto px-4 py-8">
      <div className="max-w-4xl mx-auto">
        <div className="bg-white rounded-lg shadow-lg p-8">
          {/* Quiz Header */}
          <div className="mb-6">
            <h1 className="text-3xl font-bold mb-2">{quiz.title}</h1>
            <div className="flex items-center justify-between">
              <span className="text-gray-600">
                Question {(session?.currentQuestionIndex || 0) + 1} of {quiz.metadata.questions.length}
              </span>
              {quiz.metadata.time_limit && session && (
                <Timer
                  initialTime={session.timeRemaining}
                  onTimeUp={handleTimeUp}
                />
              )}
            </div>
          </div>

          {/* Progress Bar */}
          <div className="w-full bg-gray-200 rounded-full h-2 mb-8">
            <div
              className="bg-primary h-2 rounded-full transition-all duration-300"
              style={{
                width: `${((session?.currentQuestionIndex || 0) + 1) / quiz.metadata.questions.length * 100}%`
              }}
            />
          </div>

          {/* Question */}
          {currentQuestion && session && (
            <QuizQuestion
              question={currentQuestion}
              questionNumber={session.currentQuestionIndex + 1}
              onAnswer={handleAnswer}
              disabled={timeUp}
            />
          )}
        </div>
      </div>
    </div>
  )
}